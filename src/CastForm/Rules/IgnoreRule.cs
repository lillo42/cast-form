using System;
using System.Reflection;

namespace CastForm.Rules
{
    public class IgnoreRule : IRuleMapper
    {
        private readonly MemberInfo _property;

        public IgnoreRule(MemberInfo property)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public bool Match(PropertyInfo property)
        {
            return property.Equals(_property);
        }
    }
}
