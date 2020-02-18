using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule when One is nullable and other not and is same type. like:
    ///     int Id -> int? Id
    /// </summary>
    public class NullableRuleForSameTypeWhenOneIsNullable : IRuleMapper, IRuleNeedLocalField
    {
        public NullableRuleForSameTypeWhenOneIsNullable(MemberInfo source, MemberInfo destiny)
        {
            SourceProperty = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            DestinyProperty = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));

            if (SourceProperty.PropertyType.IsNullable())
            {
                LocalFields = new []{ SourceProperty.PropertyType };
            }
        }

        public PropertyInfo DestinyProperty { get; }

        public PropertyInfo? SourceProperty { get; }

        public void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            if (DestinyProperty.PropertyType.IsNullable())
            {
                // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvJNmLOAOiJ2ANOYsBfAG4/f1RQlFRUDEwwXWBOOQAzMiViflQrDRMuXn4hNlNxaXklZWCUcKisHGzOUQy/aNjgexpMAHNOYEDJLp7wyrRquGMCnIbrBia4gH5W6g6+3u7MAaA==
                GenerateMapWithDestinyAsNullable(il);
            }
            else
            {
                // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvJNmLOAOiIOA/N8x3OADMyGQhgABpzCwBfAG4o6NRElFRUDEwwXWBOOWClYn5UKw0TLl5+ITZTcWl5JWV4lGS0rBxSzlEiqPTM4D8vGkwAc05gWMlR8eTmtFa4YyqyrusGHqz7QZGxie3poA===
                GenerateMapWithDestinyAsNotNullable(il, localFields);
            }
        }
        
        private void GenerateMapWithDestinyAsNullable(ILGenerator il)
        {
            var constructor = typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(DestinyProperty.PropertyType))
                .GetConstructors()[0];
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, SourceProperty!.GetMethod, null);
            il.Emit(OpCodes.Newobj, constructor);
            il.EmitCall(OpCodes.Callvirt, DestinyProperty.SetMethod, null);
        }


        private void GenerateMapWithDestinyAsNotNullable(ILGenerator il, IReadOnlyDictionary<Type, LocalBuilder> localField)
        {
            var getValueOrDefault = SourceProperty!.PropertyType.GetMethod("GetValueOrDefault", Type.EmptyTypes);
            var field = localField[SourceProperty.PropertyType];
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, SourceProperty.GetMethod, null);
            il.Emit(OpCodes.Stloc_S, field.LocalIndex);
            il.Emit(OpCodes.Ldloca_S, field.LocalIndex);
            il.EmitCall(OpCodes.Call, getValueOrDefault, null);
            il.EmitCall(OpCodes.Callvirt, DestinyProperty.SetMethod, null);
        }

        public IEnumerable<Type> LocalFields { get; }
    }
}
