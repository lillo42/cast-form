using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CastForm.Rules;

namespace CastForm.Generator
{
    internal partial class MapperGenerator
    {
        private readonly Type _source;
        private readonly Type _destiny;
        private readonly IEnumerable<IRuleMapper> _rules;
        private ILGenerator _ilGenerator;

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

            var type = moduleBuilder.DefineType(typeName, typeAttributes, null, interfaces);
            var methodBuilder = type.DefineMethod(nameof(IMapper.Map), 
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, 
                _destiny, new[] { _source });

            _ilGenerator = methodBuilder.GetILGenerator();
            GenerateMap();

            return type.CreateType();
        }


        private void GenerateMap()
        {
            var sourceProperties = _source.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destinyProperties = _destiny.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            _ilGenerator.Emit(OpCodes.Newobj, _destiny.GetConstructor(new Type[0]));

            foreach (var sourceProperty in sourceProperties)
            {
                var rule = _rules.FirstOrDefault(x => x.Match(sourceProperty));
                if (rule != null)
                {
                    rule.Execute(_ilGenerator);
                    continue;
                }

                var destinyProperty = destinyProperties.FirstOrDefault(x => x.Name == sourceProperty.Name && x.PropertyType == sourceProperty.PropertyType);

                if (destinyProperty == null)
                {
                    continue;
                }

                _ilGenerator.Emit(OpCodes.Dup);
                _ilGenerator.Emit(OpCodes.Ldarg_1);
                _ilGenerator.EmitCall(OpCodes.Callvirt, sourceProperty.GetMethod, null);
                _ilGenerator.EmitCall(OpCodes.Callvirt, destinyProperty.SetMethod,
                    new[] { destinyProperty.PropertyType });
            }

            _ilGenerator.Emit(OpCodes.Ret);
        }
    }
}
