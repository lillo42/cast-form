using System;
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

        public IEnumerable<Type> LocalField
        {
            get
            {
                var result = new List<Type>();

                if (_destiny.PropertyType == typeof(string))
                {
                    result.Add(_source.PropertyType);
                }
                else if(_destiny.PropertyType.IsPrimitive && _source.PropertyType.IsPrimitive)
                {
                    
                }

                return result;
            }
        }
        public bool Match(PropertyInfo property)
            => _source.Equals(property);

        public void Execute(ILGenerator il, IEnumerable<LocalBuilder> local)
            => Execute(il, local, _source, _destiny);

        internal static void Execute(ILGenerator il, IEnumerable<LocalBuilder> locals, PropertyInfo source,
            PropertyInfo destiny)
        {
            if (destiny.PropertyType == typeof(string))
            {
                ToString(il, locals, source, destiny);
            }
            else if (source.PropertyType.IsPrimitive && destiny.PropertyType.IsPrimitive)
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

        private static void ToString(ILGenerator il, IEnumerable<LocalBuilder> locals, PropertyInfo source, PropertyInfo destiny)
        {
            var local = GetLocalBuilder(locals, source.PropertyType);
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

        private static LocalBuilder GetLocalBuilder(IEnumerable<LocalBuilder> locals, Type type)
            => locals.First(x => x.LocalType == type);

        private static bool ShouldUseSameType(Type source, Type destiny)
        {
            if (destiny == typeof(int)
                || destiny == typeof(uint))
            {
                return source == typeof(int)
                       || source == typeof(uint)
                       || source == typeof(short)
                       || source == typeof(ushort)
                       || source == typeof(byte);
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
                       || source == typeof(byte);
            }

            if (destiny == typeof(short)
                || destiny == typeof(ushort))
            {
                return source == typeof(short)
                       || source == typeof(ushort)
                       || source == typeof(byte);
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
    }
}
