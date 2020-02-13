using System.Reflection;

namespace CastForm.Rules
{
    internal static class ForRuleFactory
    {
        public static IRuleMapper CreateRule(PropertyInfo destiny, PropertyInfo source)
        {
            if (source.PropertyType == destiny.PropertyType)
            {
                return new SameTypeRule(source, destiny);
            }

            if (source.PropertyType.IsNetType() && destiny.PropertyType.IsNetType())
            {
                return new DifferentNetTypeRule(source, destiny);
            }

            if ((source.PropertyType.IsNullable() && !destiny.PropertyType.IsNullable() && destiny.PropertyType.IsNetType())
                || (!source.PropertyType.IsNullable() && destiny.PropertyType.IsNullable() && source.PropertyType.IsNetType()))
            {
                var sourceType = source.PropertyType.GetUnderlyingType();
                var destinyType = destiny.PropertyType.GetUnderlyingType();
                if (sourceType == destinyType)
                {
                    return new NullableRuleForSameTypeWhenOneIsNullable(source, destiny);
                }

                return new NullableRuleForDifferentTypeWhenOneIsNullable(source, destiny);
            }

            if (source.PropertyType.IsNullable() && destiny.PropertyType.IsNullable() 
                && source.PropertyType.GetUnderlyingType().IsNetType() && destiny.PropertyType.GetUnderlyingType().IsNetType())
            {
                
                return new NullableRuleForDifferentType(source, destiny);
            }

            return new DifferentTypeRule(source,destiny);
        }
    }
}
