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
        private TypeBuilder _typeBuilder;
        private ILGenerator _ilGenerator;
        private FieldMapper _fieldMapper;

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

            _typeBuilder = moduleBuilder.DefineType(typeName, typeAttributes, null, interfaces);
            var methodBuilder = _typeBuilder.DefineMethod(nameof(IMapper.Map), 
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, 
                _destiny, new[] { _source });

            _ilGenerator = methodBuilder.GetILGenerator();
            GenerateMap();

            if (_fieldMapper.Fields.Any())
            {
                var ctor =_typeBuilder.DefineConstructor(MethodAttributes.Public, 
                    CallingConventions.Standard,
                    _fieldMapper.Fields.Keys.ToArray());

                var il = ctor.GetILGenerator();
                var counter = 1;
                foreach (var (type, field) in _fieldMapper.Fields)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_S, counter++);
                    il.Emit(OpCodes.Stfld, field);
                }

                il.Emit(OpCodes.Ret);
            }

            return _typeBuilder.CreateType();
        }


        private void GenerateMap()
        {
            var sourceProperties = _source.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destinyProperties = _destiny.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            _fieldMapper = new FieldMapper(
                DefineField(_typeBuilder, sourceProperties, destinyProperties),
                new Dictionary<Type, LocalBuilder>());

            _ilGenerator.Emit(OpCodes.Newobj, _destiny.GetConstructor(new Type[0]));

            foreach (var sourceProperty in sourceProperties)
            {
                var rule = _rules.FirstOrDefault(x => x.Match(sourceProperty));
                if (rule != null)
                {
                    rule.Execute(_ilGenerator, _fieldMapper);
                    continue;
                }

                var destinyProperty = destinyProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);

                if (destinyProperty == null)
                {
                    continue;
                }

                ForRuleFactory.CreateRule(sourceProperty, destinyProperty)
                    .Execute(_ilGenerator, _fieldMapper);
            }

            _ilGenerator.Emit(OpCodes.Ret);
        }

        private IReadOnlyDictionary<Type, FieldBuilder> DefineField(TypeBuilder builder, PropertyInfo[] sourceProperties, PropertyInfo[] destinyProperties)
        {
            var fields = new Dictionary<Type, FieldBuilder>();
            var needLocalFields = _rules.Where(x => x is IRuleNeedField).Cast<IRuleNeedField>();
            foreach (var localField in needLocalFields)
            {
                if (!fields.ContainsKey(localField.Field))
                {
                    var args = localField.Field.GetGenericArguments();
                    var fieldName = $"_map{args[0].Name}{args[1].Name}";
                    fields.Add(localField.Field, builder.DefineField(fieldName, localField.Field, FieldAttributes.Private | FieldAttributes.InitOnly));
                }
            }

            foreach (var sourceProperty in sourceProperties)
            {
                var destinyProperty = destinyProperties.FirstOrDefault(x =>
                    x.Name == sourceProperty.Name && x.PropertyType != sourceProperty.PropertyType
                    && (!x.PropertyType.IsNetType() || !sourceProperty.PropertyType.IsNetType()));

                if (destinyProperty != null)
                {
                    var type = typeof(IMap<,>).MakeGenericType(new[] { sourceProperty.PropertyType, destinyProperty.PropertyType });
                    if (!fields.ContainsKey(type))
                    {
                        var args = type.GetGenericArguments();
                        var fieldName = $"_map{args[0].Name}{args[1].Name}";
                        fields.Add(type, builder.DefineField(fieldName, type, FieldAttributes.Private | FieldAttributes.InitOnly));
                    }
                }
            }

            return fields;
        }
    }
}
