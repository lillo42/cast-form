using System;
using System.Reflection;
using System.Reflection.Emit;
using CastForm.Generator;

namespace CastForm.Rules
{
    public class ForDifferentNetTypeRule : IRuleMapper
    {
        private readonly PropertyInfo _source;
        private readonly PropertyInfo _destiny;

        public ForDifferentNetTypeRule(MemberInfo source, MemberInfo destiny)
        {
            _source = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));
        }

        public bool Match(PropertyInfo property)
            => _source.Equals(property);

        public void Execute(ILGenerator il, FieldMapper field)
            => Execute(il, field, _source, _destiny);

        internal static void Execute(ILGenerator il, FieldMapper field, PropertyInfo source, PropertyInfo destiny)
        {
            if (ShouldUseSameType(source.PropertyType, destiny.PropertyType))
            {
                ForSameTypeRule.Execute(il, source, destiny);
            }
            else
            {
                UseConverter(il, source, destiny);
            }
        }

        private static bool ShouldUseSameType(Type source, Type destiny)
        {
            if (destiny == typeof(int)
                || destiny == typeof(uint))
            {
                return source == typeof(int)
                       || source == typeof(uint)
                       || source == typeof(short)
                       || source == typeof(ushort)
                       || source == typeof(byte)
                       || source == typeof(sbyte);
            }

            if (destiny == typeof(long)
                || destiny == typeof(ulong))
            {
                return source == typeof(long)
                       || source == typeof(ulong)
                       || source == typeof(int)
                       || source == typeof(uint)
                       || source == typeof(short)
                       || source == typeof(ushort)
                       || source == typeof(byte)
                       || source == typeof(sbyte);
            }

            if (destiny == typeof(short)
                || destiny == typeof(ushort))
            {
                return source == typeof(short)
                       || source == typeof(ushort)
                       || source == typeof(byte)
                       || source == typeof(sbyte);
            }

            if (destiny == typeof(byte)
                || destiny == typeof(sbyte))
            {
                return source == typeof(byte)
                       || source == typeof(sbyte);
            }

            return false;
        }

        private static void UseConverter(ILGenerator il, PropertyInfo source, PropertyInfo destiny)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, source.GetMethod, null);
            var convert = typeof(Convert).GetRuntimeMethod(GetConvertTo(destiny.PropertyType), new[] { source.PropertyType });
            il.EmitCall(OpCodes.Call, convert, null);
            il.EmitCall(OpCodes.Callvirt, destiny.SetMethod, null);
        }

        private static string GetConvertTo(Type type)
        {
            if (type == typeof(float))
            {
                return "ToSingle";
            }

            return $"To{type.Name}";
        }
    }
}
