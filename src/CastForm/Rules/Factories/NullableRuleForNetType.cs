using System.Reflection;
using CastForm.Generator;

namespace CastForm.Rules.Factories
{
    /// <summary>
    /// Rule Factory for <see cref="NullableRuleForSameTypeWhenOneIsNullable"/> and <see cref="NullableRuleForDifferentTypeWhenOneIsNullable"/>. 
    /// </summary>
    public class NullableRuleForNetType : IRuleFactory
    {
        /// <inheritdoc />
        public bool CanCreateRule(PropertyInfo source, PropertyInfo destiny) 
            => (source.PropertyType.IsNullable() && !destiny.PropertyType.IsNullable() && destiny.PropertyType.IsNetType())
               || (!source.PropertyType.IsNullable() && destiny.PropertyType.IsNullable() && source.PropertyType.IsNetType());

        /// <inheritdoc />
        public IRuleMapper CreateRule(PropertyInfo source, PropertyInfo destiny, IHashCodeFactoryGenerator factoryGenerator)
        {
            var sourceType = source.PropertyType.GetUnderlyingType();
            var destinyType = destiny.PropertyType.GetUnderlyingType();
            if (sourceType == destinyType)
            {
                return new NullableRuleForSameTypeWhenOneIsNullable(source, destiny);
            }

            return new NullableRuleForDifferentTypeWhenOneIsNullable(source, destiny);
        }
    }
}
