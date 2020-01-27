﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    public class ForDifferentTypeRule : IRuleMapper, IRuleNeedLocalField
    {
        private readonly PropertyInfo _source;
        private readonly PropertyInfo _destiny;

        public ForDifferentTypeRule(MemberInfo source, MemberInfo destiny)
        {
            _source = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));
        }

        public IEnumerable<Type> LocalField { get; } = new List<Type>();

        public bool Match(PropertyInfo property)
            => _source.Equals(property);

        public void Execute(ILGenerator il, IDictionary<Type, LocalBuilder> local)
            => Execute(il, local, _source, _destiny);

        internal static void Execute(ILGenerator il, IDictionary<Type, LocalBuilder> locals, PropertyInfo source,
            PropertyInfo destiny)
        {
            if (IsNetType(source.PropertyType) && IsNetType(destiny.PropertyType))
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
        }

        private static void ToString(ILGenerator il, IDictionary<Type, LocalBuilder> locals, PropertyInfo source, PropertyInfo destiny)
        {
            var local = locals[source.PropertyType];
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, source.GetMethod, null);
            il.Emit(OpCodes.Stloc_S, local);
            il.Emit(OpCodes.Ldloca_S, local);
            var toString = source.PropertyType.GetMethod(nameof(ToString),
                BindingFlags.Public | BindingFlags.Instance, null,
                CallingConventions.Any, new Type[0], null);
            var call = source.PropertyType.IsPrimitive switch
            {
                true => OpCodes.Call,
                false => OpCodes.Callvirt
            };
            il.EmitCall(call, toString, null);
            il.EmitCall(OpCodes.Callvirt, destiny.SetMethod, null);
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
            var convert = typeof(Convert).GetRuntimeMethod($"To{destiny.PropertyType.Name}", new [] {source.PropertyType});
            il.EmitCall(OpCodes.Call, convert, null);
            il.EmitCall(OpCodes.Callvirt, destiny.SetMethod, null);
        }

        private static bool IsNetType(Type type) 
            => type.IsPrimitive
               || type == typeof(string)
               || type == typeof(DateTime)
               || type == typeof(decimal);
    }
}
