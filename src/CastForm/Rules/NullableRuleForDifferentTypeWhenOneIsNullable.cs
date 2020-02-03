using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    public class NullableRuleForDifferentTypeWhenOneIsNullable : IRuleMapper, IRuleNeedLocalField
    {
        private readonly PropertyInfo _source;
        private readonly PropertyInfo _destiny;

        public NullableRuleForDifferentTypeWhenOneIsNullable(MemberInfo source, MemberInfo destiny)
        {
            _source = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));

            if (_source.PropertyType.IsNullable())
            {
                LocalFields = new [] { _source.PropertyType } ;
            }
        }

        public bool Match(PropertyInfo property)
            => _source.Equals(property);


        public void Execute(ILGenerator il, IReadOnlyDictionary<Type, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            if (_destiny.PropertyType.IsNullable())
            {
                GenerateMapWithDestinyAsNullable(il);
            }
            else
            {
                GenerateMapWithDestinyAsNotNullable(il, localFields);
            }
        }
        
        private void GenerateMapWithDestinyAsNullable(ILGenerator il)
        {
            var constructor = typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(_destiny.PropertyType)).GetConstructors()[0];
            var convert = typeof(Convert).GetRuntimeMethod(GetConvertTo(_destiny.PropertyType), new[] { _source.PropertyType });

            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, _source.GetMethod, null);
            il.EmitCall(OpCodes.Call, convert, null);
            il.Emit(OpCodes.Newobj, constructor);
            il.EmitCall(OpCodes.Callvirt, _destiny.SetMethod, null);
        }


        private void GenerateMapWithDestinyAsNotNullable(ILGenerator il, IReadOnlyDictionary<Type, LocalBuilder> localField)
        {
            var getValueOrDefault = _source.PropertyType.GetMethod("GetValueOrDefault", Type.EmptyTypes);
            var field = localField[_source.PropertyType];
            var convert = typeof(Convert).GetRuntimeMethod(GetConvertTo(_destiny.PropertyType), new[] { _source.PropertyType.GetUnderlyingType() });

            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, _source.GetMethod, null);
            il.Emit(OpCodes.Stloc_S, field.LocalIndex);
            il.Emit(OpCodes.Ldloca_S, field.LocalIndex);
            il.EmitCall(OpCodes.Call, getValueOrDefault, null);
            il.EmitCall(OpCodes.Call, convert, null);
            il.EmitCall(OpCodes.Callvirt, _destiny.SetMethod, null);
        }

        private static string GetConvertTo(Type type)
        {
            type = type.GetUnderlyingType();

            if (type == typeof(float))
            {
                return "ToSingle";
            }

            return $"To{type.Name}";
        }

        public IEnumerable<Type> LocalFields { get; }
    }
}
