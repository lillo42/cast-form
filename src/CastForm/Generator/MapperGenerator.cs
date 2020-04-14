using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CastForm.Rules;

namespace CastForm.Generator
{
    /// <summary>
    /// Generate <see cref="IMap"/> for define source and destiny.
    /// </summary>
    public class MapperGenerator
    {
        private readonly Type _source;
        private readonly Type _destiny;

        private readonly PropertyInfo[] _sourceProperties;
        private readonly PropertyInfo[] _destinyProperties;
        private readonly IEnumerable<IRuleMapper> _rules;

        private readonly HashCodeFactoryGenerator _hashCodeFactoryGenerator;

        private const string s_serviceProvider = "_serviceProvider";
        private const string s_isInit = "_isInit";

        private static readonly MethodInfo s_getService = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService));
        private static readonly MethodInfo s_getTypeFromHandle = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle));

        /// <summary>
        /// Initialize a new instance of <see cref="MapperGenerator"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destiny"></param>
        /// <param name="rules"></param>
        /// <param name="hashCodeFactoryGenerator"></param>
        public MapperGenerator(Type source, Type destiny, IEnumerable<IRuleMapper> rules, HashCodeFactoryGenerator hashCodeFactoryGenerator)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny ?? throw new ArgumentNullException(nameof(destiny));
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
            _hashCodeFactoryGenerator = hashCodeFactoryGenerator;

            _sourceProperties = source.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _destinyProperties = destiny.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Build  <see cref="IMap"/>
        /// </summary>
        /// <returns>Generated <see cref="IMap"/></returns>
        public Type Generate(IEnumerable<MapperProperty> mapperProperties)
        {
            var typeName = $"{_source.Name}To{_destiny.Name}Mapper";

            var assemblyName = new AssemblyName($"{typeName}Assembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule($"{typeName}Module");
            const TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit;

            var mapper = typeof(IMap<,>).MakeGenericType(_source, _destiny);
            var interfaces = new[] { mapper, typeof(IMap) };

            var typeBuilder = moduleBuilder.DefineType(typeName, typeAttributes, null, interfaces);
            var methodBuilder = typeBuilder.DefineMethod(nameof(IMapper.Map),
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                _destiny, new[] { _source });

            var generator = methodBuilder.GetILGenerator();

            var rules = CreateRules();
            var fields = DefineField(typeBuilder, rules);
            var localFields = DefineLocalField(generator, rules);
            
            CreateConstructor(typeBuilder, fields);
            var init = CreateInit(typeBuilder, fields);
            GenerateMap(generator, rules, fields, localFields, mapperProperties, init);

            return typeBuilder.CreateType();
        }

        private static void CreateConstructor(TypeBuilder typeBuilder, IReadOnlyDictionary<string, FieldBuilder> fields)
        {
            if (fields.Any())
            {
                var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public,
                    CallingConventions.Standard,
                    new []{ typeof(IServiceProvider)});

                var il = ctor.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, fields[s_serviceProvider]);
                il.Emit(OpCodes.Ret);
            }
        }

        private static MethodBuilder? CreateInit(TypeBuilder typeBuilder, IReadOnlyDictionary<string, FieldBuilder> fields)
        {
            if (fields.Any())
            {
                // https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMDeqmJmADgE5gBuAhsAKaYUO0AmA9gHYQCemASQDKDCtTABjBgAUKHcW1GYA+gGcA3MVKUa9JgCMOHCCrCqBXMME0pS5KnUaCAgpg427Ox0wEAhTPoe2g562AAsgpbAABQAlJhaJES2dqRgAGbRAITKZhZWsYmpyamlbpgAvJjRAs6xagB0AOIMwCJikgzRwLxkDByZtbGxQWWk+pXVfvWqza3t4lLdvf2DvsOjY6bmUZPAFACuDJt2AL5F5yiXqBiYYFyMFOm0UoILnbLyYIoUqCWkHH0ACsGBJgJgWm1RIsugAVFaYVTQzrwvojVDXNBYe6PZ6vWp/TE3bEPUR4ny+QlAA===
                var provider = fields[s_serviceProvider];
                var init = typeBuilder.DefineMethod("Init", MethodAttributes.Private);
                var il = init.GetILGenerator();

                var next = il.DefineLabel();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, fields[s_isInit]);
                il.Emit(OpCodes.Brtrue_S, next);

                foreach (var (name, field) in fields)
                {
                    if (name == s_serviceProvider || name == s_isInit)
                    {
                        continue;
                    }

                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, provider);
                    il.Emit(OpCodes.Ldtoken, field.FieldType);
                    il.EmitCall(OpCodes.Call, s_getTypeFromHandle, null);
                    il.EmitCall(OpCodes.Callvirt, s_getService, null);
                    il.Emit(OpCodes.Castclass, field.FieldType);
                    il.Emit(OpCodes.Stfld, field);
                }

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Stfld, fields[s_isInit]);

                il.MarkLabel(next);
                il.Emit(OpCodes.Ret);

                return init;
            }

            return null;
        }


        private void GenerateMap(ILGenerator generator,
            IEnumerable<IRuleMapper> rules,
            IReadOnlyDictionary<string, FieldBuilder> fields,
            IReadOnlyDictionary<Type, LocalBuilder> localFields,
            IEnumerable<MapperProperty> mapperProperties,
            MethodBuilder? builder)
        {
            if (builder != null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.EmitCall(OpCodes.Call, builder, null);
            }


            generator.Emit(OpCodes.Newobj, _destiny.GetConstructor(Type.EmptyTypes));

            foreach (var destinyProperty in _destinyProperties)
            {
                var rule = rules.First(x => x.Match(destinyProperty));
                rule.Execute(generator, fields, localFields, mapperProperties);
            }

            generator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Create rules for source and destiny properties.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IRuleMapper> CreateRules() 
            => CreateRules(_sourceProperties, _destinyProperties);

        private IEnumerable<IRuleMapper> CreateRules(PropertyInfo[] sourceProperties, PropertyInfo[] destinyProperties)
        {
            var rules = new LinkedList<IRuleMapper>();
            foreach (var destinyProperty in destinyProperties)
            {
                var rule = _rules.FirstOrDefault(x => x.Match(destinyProperty));
                if (rule != null)
                {
                    rules.AddLast(rule);
                    continue;
                }

                var sourceProperty = sourceProperties.FirstOrDefault(x => x.Name == destinyProperty.Name);

                if (sourceProperty == null)
                {
                    continue;
                }

                rules.AddLast(ForRuleFactory.CreateRule(destinyProperty, sourceProperty, _hashCodeFactoryGenerator));
            }

            return rules;
        }

        private static IReadOnlyDictionary<string, FieldBuilder> DefineField(TypeBuilder builder, IEnumerable<IRuleMapper> rules)
        {
            var fields = new Dictionary<string, FieldBuilder>();

            var needLocalFields = rules.Where(x => x is IRuleNeedField).Cast<IRuleNeedField>().ToArray();

            if (needLocalFields.Any())
            {
                fields.Add(s_serviceProvider, builder.DefineField(s_serviceProvider, typeof(IServiceProvider), FieldAttributes.Private | FieldAttributes.InitOnly));
                fields.Add(s_isInit, builder.DefineField(s_isInit, typeof(bool), FieldAttributes.Private));
            }

            foreach (var localField in needLocalFields)
            {
                foreach (var (name, type) in localField.Fields)
                {
                    if (!fields.ContainsKey(name))
                    {
                        fields.Add(name, builder.DefineField(name, type, FieldAttributes.Private));
                    }
                }
            }

            return fields;
        }

        private static IReadOnlyDictionary<Type, LocalBuilder> DefineLocalField(ILGenerator generator, IEnumerable<IRuleMapper> rules)
        {
            var fields = new Dictionary<Type, LocalBuilder>();
            var needLocalFields = rules.Where(x => x is IRuleNeedLocalField).Cast<IRuleNeedLocalField>();
            foreach (var localField in needLocalFields)
            {
                foreach (var field in localField.LocalFields ?? Enumerable.Empty<Type>())
                {
                    if (field != null && !fields.ContainsKey(field))
                    {
                        fields.Add(field, generator.DeclareLocal(field));
                    }
                }

            }

            return fields;
        }
    }
}
