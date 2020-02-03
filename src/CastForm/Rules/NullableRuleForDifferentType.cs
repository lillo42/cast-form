using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    public class NullableRuleForDifferentType : IRuleMapper, IRuleNeedLocalField
    {
        private readonly PropertyInfo _source;
        private readonly PropertyInfo _destiny;

        public NullableRuleForDifferentType(MemberInfo source, MemberInfo destiny)
        {
            _source = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));

            LocalFields = new[] { _source.PropertyType, _destiny.PropertyType };
        }

        public bool Match(PropertyInfo property)
            => _source.Equals(property);


        public void Execute(ILGenerator il, IReadOnlyDictionary<Type, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            // based on: https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMDeqmJ2WAymALYAOEApgEKYCyAFJbQwIKYDOAewCuAJwDG9AJSFipOTADsmAHb0A7pk50mMlHP2kAckKoAjeiMwBefsPH0AdAEkAJg4ASAQz4A1TxCF6TAB+FXVMCAFlAHNgtlwogDcLYAcAFQEnZWAANgAWNkFRCWcXSWkQFSEICFkDAF8AbjrMetQ2lFQMbDhNam1uVCI9Um6wbNDXQkxo+mBG/jmFjvksPgALARFgULT6AA9gadn5xdOOjq6sHC0GRiGW7siY0OMzC2Ols+XHrFMAT2A9FCfgCQQIMy+fC+FyAA=
            var constructor = typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(_destiny.PropertyType)).GetConstructors()[0];
            var sourceField = localFields[_source.PropertyType];
            var destinyField = localFields[_destiny.PropertyType];
            var hasValue = _source.PropertyType.GetProperty("HasValue", BindingFlags.Public | BindingFlags.Instance);
            var getValue = _source.PropertyType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            var convert = typeof(Convert).GetRuntimeMethod(GetConvertTo(_destiny.PropertyType), new[] { _source.PropertyType.GetUnderlyingType() });

            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, _source.GetMethod, null);
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
            il.EmitCall(OpCodes.Callvirt, _source.GetMethod, null);
            il.Emit(OpCodes.Stloc_S, sourceField.LocalIndex);
            il.Emit(OpCodes.Ldloca_S, sourceField.LocalIndex);
            il.EmitCall(OpCodes.Call, getValue.GetMethod, null);
            il.EmitCall(OpCodes.Call, convert, null);
            il.Emit(OpCodes.Newobj, constructor);

            il.MarkLabel(setValue);
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
