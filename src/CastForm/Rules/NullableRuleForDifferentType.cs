using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule when both property are nullable and different type. like:
    ///     int? Id -> long? Id
    /// </summary>
    public class NullableRuleForDifferentType : IRuleMapper, IRuleNeedLocalField
    {
        /// <summary>
        /// Initialize a new instance of <see cref="NullableRuleForDifferentType"/>
        /// </summary>
        /// <param name="source">The source member</param>
        /// <param name="destiny">The source member</param>
        public NullableRuleForDifferentType(MemberInfo source, MemberInfo destiny)
        {
            SourceProperty = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            DestinyProperty = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));

            LocalFields = new[] { SourceProperty.PropertyType, DestinyProperty.PropertyType };
        }

        /// <summary>
        /// Property Destiny
        /// </summary>
        public PropertyInfo DestinyProperty { get; }

        /// <summary>
        /// Property Source
        /// </summary>
        public PropertyInfo? SourceProperty { get; }

        /// <summary>
        /// Execute rule
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> that generate method  </param>
        /// <param name="fields">The <see cref="IReadOnlyDictionary{TKey, TValue}"/> that have all field in class that was already added.</param>
        /// <param name="localFields">The <see cref="IReadOnlyDictionary{TKey, TValue}"/> that have all locals field that was already added.</param>
        /// <param name="mapperProperties">The <see cref="IEnumerable{MapperProperty}"/> that have map.</param>
        public void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields, IEnumerable<MapperProperty> mapperProperties)
        {
            // based on: https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMDeqmJ2WAymALYAOEApgEKYCyAFJbQwIKYDOAewCuAJwDG9AJSFipOTADsmAHb0A7pk50mMlHP2kAckKoAjeiMwBefsPH0AdAEkAJg4ASAQz4A1TxCF6TAB+FXVMCAFlAHNgtlwogDcLYAcAFQEnZWAANgAWNkFRCWcXSWkQFSEICFkDAF8AbjrMetQ2lFQMbDhNam1uVCI9Um6wbNDXQkxo+mBG/jmFjvksPgALARFgULT6AA9gadn5xdOOjq6sHC0GRiGW7siY0OMzC2Ols+XHrFMAT2A9FCfgCQQIMy+fC+FyAA=
            // public class Map
            // {
            //      public SimpleB Map(SimpleA source)
            //      {
            //          return new SimpleB
            //          {
            //              Long = source.Id.HasValue ? new long?(Convert.ToInt64(source.Int)) : null // This mapper is create this line
            //          };
            //      }
            // }
            // public class SimpleA
            // {
            //      public int? Int { get; set;}
            // }
            // public class SimpleB
            // {
            //      public long? Long { get; set;}
            // }

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

        /// <summary>
        /// Local filed used in Method
        /// </summary>
        public IEnumerable<Type> LocalFields { get; }
    }
}
