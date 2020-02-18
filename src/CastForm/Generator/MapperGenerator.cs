using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CastForm.Rules;

namespace CastForm.Generator
{
    /// <summary>
    /// Generate Map for define source and destiny.
    /// </summary>
    public class MapperGenerator
    {
        private readonly Type _source;
        private readonly Type _destiny;

        private readonly PropertyInfo[] _sourceProperties;
        private readonly PropertyInfo[] _destinyProperties;
        private readonly IEnumerable<IRuleMapper> _rules;

        public MapperGenerator(Type source, Type destiny, IEnumerable<IRuleMapper> rules)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny ?? throw new ArgumentNullException(nameof(destiny));
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));

            _sourceProperties = source.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _destinyProperties = destiny.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Build IMap
        /// </summary>
        /// <returns>Generated <see cref="IMap"/></returns>
        public Type Generate()
        {
            var typeName = $"{_source.Name}To{_destiny.Name}Mapper";

            var assemblyName = new AssemblyName($"{typeName}Assembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule($"{typeName}Module");
            const TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit;

            var mapper = typeof(IMap<,>).MakeGenericType(new[] { _source, _destiny });
            var interfaces = new[] { mapper, typeof(IMap) };

            var typeBuilder = moduleBuilder.DefineType(typeName, typeAttributes, null, interfaces);
            var methodBuilder = typeBuilder.DefineMethod(nameof(IMapper.Map),
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                _destiny, new[] { _source });

            var generator = methodBuilder.GetILGenerator();

            var rules = CreateRules();
            var fields = DefineField(typeBuilder, rules);
            var localFields = DefineLocalField(generator, rules);

            BeforeExecuteRule(generator, rules, fields, localFields);
            GenerateMap(generator, rules, fields, localFields);
            AfterExecuteRule(generator, rules, fields, localFields);

            CreateConstructor(typeBuilder, fields);

            return typeBuilder.CreateType();
        }

        private static void CreateConstructor(TypeBuilder typeBuilder, IReadOnlyDictionary<string, FieldBuilder> fields)
        {
            if (fields.Any())
            {
                var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public,
                    CallingConventions.Standard,
                    fields.Values.Select(x => x.FieldType).ToArray());

                var il = ctor.GetILGenerator();
                var counter = 1;
                foreach (var (_, field) in fields)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_S, counter++);
                    il.Emit(OpCodes.Stfld, field);
                }

                il.Emit(OpCodes.Ret);
            }
        }

        private static void BeforeExecuteRule(ILGenerator il,
            IEnumerable<IRuleMapper> rules,
            IReadOnlyDictionary<string, FieldBuilder> fields,
            IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            foreach (var rule in rules.Where(x => x is IBeforeRule).Cast<IBeforeRule>())
            {
                rule.Execute(il, fields, localFields);
            }
        }

        private static void AfterExecuteRule(ILGenerator il,
            IEnumerable<IRuleMapper> rules,
            IReadOnlyDictionary<string, FieldBuilder> fields,
            IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            foreach (var rule in rules.Where(x => x is IAfterRule).Cast<IAfterRule>())
            {
                rule.Execute(il, fields, localFields);
            }
        }

        private void GenerateMap(ILGenerator generator,
            IEnumerable<IRuleMapper> rules,
            IReadOnlyDictionary<string, FieldBuilder> fields,
            IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            generator.Emit(OpCodes.Newobj, _destiny.GetConstructor(Type.EmptyTypes));

            foreach (var destinyProperty in _destinyProperties)
            {
                var rule = rules.First(x => x.Match(destinyProperty));
                rule.Execute(generator, fields, localFields);
            }

            generator.Emit(OpCodes.Ret);
        }

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

                rules.AddLast(ForRuleFactory.CreateRule(destinyProperty, sourceProperty));
            }

            return rules;
        }

        private static IReadOnlyDictionary<string, FieldBuilder> DefineField(TypeBuilder builder, IEnumerable<IRuleMapper> rules)
        {
            var fields = new Dictionary<string, FieldBuilder>();
            var needLocalFields = rules.Where(x => x is IRuleNeedField).Cast<IRuleNeedField>();
            foreach (var localField in needLocalFields)
            {
                foreach (var (name, type) in localField.Fields)
                {
                    if (!fields.ContainsKey(name))
                    {
                        fields.Add(name, builder.DefineField(name, type, FieldAttributes.Private | FieldAttributes.InitOnly));
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
