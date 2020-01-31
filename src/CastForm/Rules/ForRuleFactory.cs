using System;
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

            if ((source.PropertyType.IsNullable() && !destiny.PropertyType.IsNullable() && destiny.PropertyType.IsNetType())
                || (!source.PropertyType.IsNullable() && destiny.PropertyType.IsNullable() && source.PropertyType.IsNetType()))
            {
                var sourceType = source.PropertyType.GetUnderlyingType();
                var destinyType = destiny.PropertyType.GetUnderlyingType();
                if (sourceType == destinyType)
                {
                    return new ForRuleNullableWithSameType(source, destiny);
                }
            }

            return new ForDifferentTypeRule(source,destiny);
        }
    }
}
