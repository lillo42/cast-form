using System.Reflection;
using CastForm.Generator;

namespace CastForm.Rules.Factories
{
    /// <summary>
    /// Rule Factory for <see cref="NullableRuleForSameTypeWhenOneIsNullable"/> and <see cref="NullableRuleForDifferentTypeWhenOneIsNullable"/>. 
    /// </summary>
    public class NullableRuleForNetTypeOneIsNullableFactory : IRuleFactory
    {
        /// <inheritdoc />
        public bool CanCreateRule(PropertyInfo source, PropertyInfo destiny)
        {
            var sourceIsNullable = source.PropertyType.IsNullable();
            var destinyIsNullable = destiny.PropertyType.IsNullable();
            
            return source.PropertyType.GetUnderlyingType().IsNetType() && destiny.PropertyType.GetUnderlyingType().IsNetType() 
                        && ((sourceIsNullable && !destinyIsNullable) || (!sourceIsNullable && destinyIsNullable)); 
        }

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
