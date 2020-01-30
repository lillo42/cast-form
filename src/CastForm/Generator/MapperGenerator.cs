using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CastForm.Rules;

namespace CastForm.Generator
{
    internal class MapperGenerator
    {
        private readonly Type _source;
        private readonly Type _destiny;
        private readonly IEnumerable<IRuleMapper> _rules;

        public MapperGenerator(Type source, Type destiny, IEnumerable<IRuleMapper> rules)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny ?? throw new ArgumentNullException(nameof(destiny));
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
        }

        public Type Generate()
        {
            var typeName = $"{_source.Name}To{_destiny.Name}Mapper";

            var assemblyName = new AssemblyName($"{typeName}Assembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule($"{typeName}Module");
            const TypeAttributes typeAttributes = TypeAttributes.Class| TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit;

            var mapper = typeof(IMap<,>).MakeGenericType(new[] { _source, _destiny });
            var interfaces = new[] { mapper, typeof(IMap) };

            var typeBuilder = moduleBuilder.DefineType(typeName, typeAttributes, null, interfaces);
            var methodBuilder = typeBuilder.DefineMethod(nameof(IMapper.Map), 
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, 
                _destiny, new[] { _source });

            var generator = methodBuilder.GetILGenerator();

            var sourceProperties = _source.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destinyProperties = _destiny.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var rules = GetAllRules(sourceProperties, destinyProperties);
            var fields = DefineField(typeBuilder, rules);

            GenerateMap(generator, rules, fields, _destiny, sourceProperties, destinyProperties);

            CreateConstructor(typeBuilder, fields);

            return typeBuilder.CreateType();
        }

        private static void CreateConstructor(TypeBuilder typeBuilder, IReadOnlyDictionary<Type, FieldBuilder> fields)
        {
            if (fields.Any())
            {
                var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public,
                    CallingConventions.Standard,
                    fields.Keys.ToArray());

                var il = ctor.GetILGenerator();
                var counter = 1;
                foreach (var (type, field) in fields)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_S, counter++);
                    il.Emit(OpCodes.Stfld, field);
                }

                il.Emit(OpCodes.Ret);
            }
        }


        private static void GenerateMap(ILGenerator generator,
            IEnumerable<IRuleMapper> rules,
            IReadOnlyDictionary<Type, FieldBuilder> fields,
            Type destiny,
            PropertyInfo[] sourceProperties, 
            PropertyInfo[] destinyProperties)
        {

            var localFields = DefineLocalField(generator, rules);
            generator.Emit(OpCodes.Newobj, destiny.GetConstructor(Type.EmptyTypes));

            foreach (var sourceProperty in sourceProperties)
            {
                var rule = rules.First(x => x.Match(sourceProperty));
                rule.Execute(generator, fields, localFields);
            }

            generator.Emit(OpCodes.Ret);
        }


        private IEnumerable<IRuleMapper> GetAllRules(PropertyInfo[] sourceProperties, PropertyInfo[] destinyProperties)
        {
            var rules = new LinkedList<IRuleMapper>();
            foreach (var sourceProperty in sourceProperties)
            {
                var rule = _rules.FirstOrDefault(x => x.Match(sourceProperty));
                if (rule != null)
                {
                    rules.AddLast(rule);
                    continue;
                }

                var destinyProperty = destinyProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);

                if (destinyProperty == null)
                {
                    continue;
                }

                rules.AddLast(ForRuleFactory.CreateRule(sourceProperty, destinyProperty));
            }

            return rules;
        }

        private static IReadOnlyDictionary<Type, FieldBuilder> DefineField(TypeBuilder builder, IEnumerable<IRuleMapper> rules)
        {
            var fields = new Dictionary<Type, FieldBuilder>();
            var needLocalFields = rules.Where(x => x is IRuleNeedField).Cast<IRuleNeedField>();
            foreach (var localField in needLocalFields)
            {
                if (!fields.ContainsKey(localField.Field))
                {
                    var args = localField.Field.GetGenericArguments();
                    var fieldName = $"_map{args[0].Name}{args[1].Name}";
                    fields.Add(localField.Field, builder.DefineField(fieldName, localField.Field, FieldAttributes.Private | FieldAttributes.InitOnly));
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
                if (localField.LocalField != null && !fields.ContainsKey(localField.LocalField))
                {
                    fields.Add(localField.LocalField, generator.DeclareLocal(localField.LocalField));
                }
            }

            return fields;
        }
    }
}
