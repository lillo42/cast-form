using System.Reflection;

namespace CastForm.Rules
{
    internal static class ForRuleFactory
    {
        public static IRuleMapper CreateRule(PropertyInfo source, PropertyInfo destiny)
        {
            if (source.PropertyType == destiny.PropertyType)
            {
                return new ForSameTypeRule(source, destiny);
            }

            if (source.PropertyType.IsNetType() && destiny.PropertyType.IsNetType())
            {
                return new ForDifferentNetTypeRule(source, destiny);
            }

            return new ForDifferentTypeRule(source,destiny);
        }
    }
}
