using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule when both are nullable and different type. like:
    ///     int? Id -> long? Id
    /// </summary>
    public class NullableRuleForDifferentType : IRuleMapper, IRuleNeedLocalField
    {
        public NullableRuleForDifferentType(MemberInfo source, MemberInfo destiny)
        {
            SourceProperty = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            DestinyProperty = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));

            LocalFields = new[] { SourceProperty.PropertyType, DestinyProperty.PropertyType };
        }

        public PropertyInfo DestinyProperty { get; }

        public PropertyInfo? SourceProperty { get; }


        public void Execute(ILGenerator il, IReadOnlyDictionary<Type, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            // based on: https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMDeqmJ2WAymALYAOEApgEKYCyAFJbQwIKYDOAewCuAJwDG9AJSFipOTADsmAHb0A7pk50mMlHP2kAckKoAjeiMwBefsPH0AdAEkAJg4ASAQz4A1TxCF6TAB+FXVMCAFlAHNgtlwogDcLYAcAFQEnZWAANgAWNkFRCWcXSWkQFSEICFkDAF8AbjrMetQ2lFQMbDhNam1uVCI9Um6wbNDXQkxo+mBG/jmFjvksPgALARFgULT6AA9gadn5xdOOjq6sHC0GRiGW7siY0OMzC2Ols+XHrFMAT2A9FCfgCQQIMy+fC+FyAA=
            var constructor = typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(DestinyProperty.PropertyType)).GetConstructors()[0];
            var sourceField = localFields[SourceProperty!.PropertyType];
            var destinyField = localFields[DestinyProperty.PropertyType];
            var hasValue = SourceProperty.PropertyType.GetProperty("HasValue", BindingFlags.Public | BindingFlags.Instance);
            var getValue = SourceProperty.PropertyType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            var convert = typeof(Convert).GetRuntimeMethod(GetConvertTo(DestinyProperty.PropertyType), new[] { SourceProperty.PropertyType.GetUnderlyingType() });

            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, SourceProperty.GetMethod, null);
            il.Emit(OpCodes.Stloc_S, sourceField.LocalIndex);
            il.Emit(OpCodes.Ldloca_S, sourceField.LocalIndex);
            il.EmitCall(OpCodes.Call, hasValue.GetMethod, null);
            var @if = il.DefineLabel();
            il.Emit(OpCodes.Brtrue_S, @if);

            il.Emit(OpCodes.Ldloca_S, destinyField.LocalIndex);
            il.Emit(OpCodes.Initobj, destinyField.LocalType);
            il.Emit(OpCodes.Ldloc_S, destinyField.LocalIndex);
            var setValue = il.DefineLabel();
            il.Emit(OpCodes.Br_S, setValue);

            il.MarkLabel(@if);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, SourceProperty.GetMethod, null);
            il.Emit(OpCodes.Stloc_S, sourceField.LocalIndex);
            il.Emit(OpCodes.Ldloca_S, sourceField.LocalIndex);
            il.EmitCall(OpCodes.Call, getValue.GetMethod, null);
            il.EmitCall(OpCodes.Call, convert, null);
            il.Emit(OpCodes.Newobj, constructor);

            il.MarkLabel(setValue);
            il.EmitCall(OpCodes.Callvirt, DestinyProperty.SetMethod, null);
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
