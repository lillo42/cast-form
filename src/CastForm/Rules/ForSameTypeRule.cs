using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    public class ForSameTypeRule : IRuleMapper
    {
        private readonly PropertyInfo _source;
        private readonly PropertyInfo _destiny;

        public ForSameTypeRule(MemberInfo source, MemberInfo destiny)
        {
            _source = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));
        }


        public bool Match(PropertyInfo property) 
            => _source.Equals(property);

        public void Execute(ILGenerator il)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, _source.GetMethod, null);
            il.EmitCall(OpCodes.Callvirt, _source.SetMethod, new[] { _destiny.PropertyType });
        }
    }
}
