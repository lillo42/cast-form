using System.Reflection;
using CastForm.Generator;

namespace CastForm.Rules.Factories
{
    /// <summary>
    /// Rule Factory for <see cref="NullableRuleForSameTypeWhenOneIsNullable"/> and <see cref="NullableRuleForDifferentType"/>. 
    /// </summary>
    public class NullableRuleForDifferentTypeFactory : IRuleFactory
    {
        /// <inheritdoc />
        public bool CanCreateRule(PropertyInfo source, PropertyInfo destiny) 
            => source.PropertyType.IsNullable() && destiny.PropertyType.IsNullable() 
                                                && source.PropertyType.GetUnderlyingType().IsNetType() 
                                                && destiny.PropertyType.GetUnderlyingType().IsNetType();

        /// <inheritdoc />
        public IRuleMapper CreateRule(PropertyInfo source, PropertyInfo destiny, IHashCodeFactoryGenerator factoryGenerator) 
            => new NullableRuleForDifferentType(source, destiny);
    }
}
