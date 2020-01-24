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

            LocalField = new List<Type>
            {
                _source.PropertyType
            };
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
                il.EmitCall(OpCodes.Callvirt, destiny.SetMethod, new[] { destiny.PropertyType });
            }
        }

        private static LocalBuilder GetLocalBuilder(IEnumerable<LocalBuilder> locals, Type type)
            => locals.First(x => x.LocalType == type);

        public IEnumerable<Type> LocalField { get; }
    }
}
