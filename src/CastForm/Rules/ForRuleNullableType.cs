using System;
using System.Reflection;
using System.Reflection.Emit;
using CastForm.Generator;

namespace CastForm.Rules
{
    public class ForRuleNullableType : IRuleMapper, IRuleNeedLocalField
    {
        private readonly PropertyInfo _source;
        private readonly PropertyInfo _destiny;

        public ForRuleNullableType(MemberInfo source, MemberInfo destiny)
        {
            _source = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));

            if (_source.PropertyType.IsNullable())
            {
                LocalField = typeof(Nullable<>).MakeGenericType(_source.PropertyType);
            }
        }

        public bool Match(PropertyInfo property)
            => _source.Equals(property);


        public void Execute(ILGenerator il, FieldMapper local)
        {
            if (_destiny.PropertyType.IsNullable())
            {
                GenerateMapWithDestinyAsNullable(il);
            }
            else
            {
                GenerateMapWithDestinyAsNotNullable(il);
            }
        }


        private void GenerateMapWithDestinyAsNullable(ILGenerator il)
        {
            var constructor = typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(_destiny.PropertyType))
                .GetConstructors()[0];
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, _source.GetMethod, null);
            il.Emit(OpCodes.Newobj, constructor);
            il.EmitCall(OpCodes.Callvirt, _destiny.SetMethod, null);
        }


        private void GenerateMapWithDestinyAsNotNullable(ILGenerator il)
        {
            var constructor = typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(_destiny.PropertyType))
                .GetConstructors()[0];
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, _source.GetMethod, null);
            il.Emit(OpCodes.Newobj, constructor);
            il.EmitCall(OpCodes.Callvirt, _destiny.SetMethod, null);
        }

        public Type LocalField { get; }
    }
}
