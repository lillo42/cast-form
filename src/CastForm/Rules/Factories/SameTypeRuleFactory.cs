using System.Reflection;
using CastForm.Generator;

namespace CastForm.Rules.Factories
{
    /// <summary>
    /// Rule Factory for <see cref="SameTypeRule"/>
    /// </summary>
    public class SameTypeRuleFactory : IRuleFactory
    {
        /// <inheritdoc />
        public bool CanCreateRule(PropertyInfo source, PropertyInfo destiny) 
            => source.PropertyType == destiny.PropertyType;

        /// <inheritdoc />
        public IRuleMapper CreateRule(PropertyInfo source, PropertyInfo destiny, IHashCodeFactoryGenerator factoryGenerator) 
            => new SameTypeRule(source, destiny);
    }
}
