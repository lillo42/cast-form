using System.Reflection;
using CastForm.Generator;

namespace CastForm.Rules.Factories
{
    /// <summary>
    /// Rule Factory for <see cref="DifferentNetTypeRule"/>
    /// </summary>
    public class DifferentNetTypeRuleFactory : IRuleFactory
    {
        /// <inheritdoc />
        public bool CanCreateRule(PropertyInfo source, PropertyInfo destiny) 
            => source.PropertyType.IsNetType() && destiny.PropertyType.IsNetType();

        /// <inheritdoc />
        public IRuleMapper CreateRule(PropertyInfo source, PropertyInfo destiny, IHashCodeFactoryGenerator factoryGenerator) 
            => new DifferentNetTypeRule(source, destiny);
    }
}
