using System;
using System.Reflection;
using CastForm.Generator;

namespace CastForm.Rules.Factories
{
    /// <summary>
    /// Create Rules
    /// </summary>
    public interface IRuleFactory
    {
        /// <summary>
        /// If the <paramref name="source"/> and <paramref name="destiny"/> match with <see cref="IRuleMapper"/>.
        /// </summary>
        /// <param name="source">The source <see cref="PropertyInfo"/>.</param>
        /// <param name="destiny">The destiny <see cref="PropertyInfo"/>.</param>
        /// <returns>true if should create <see cref="IRuleMapper"/> otherwise return false.</returns>
        bool CanCreateRule(PropertyInfo source, PropertyInfo destiny);

        /// <summary>
        /// Create a new instance of <see cref="IRuleMapper"/>
        /// </summary>
        /// <param name="source">The source <see cref="PropertyInfo"/>.</param>
        /// <param name="destiny">The destiny <see cref="PropertyInfo"/></param>
        /// <param name="factoryGenerator">The <see cref="IHashCodeFactoryGenerator"/>.</param>
        /// <returns>New instance of <see cref="IRuleMapper"/></returns>
        IRuleMapper CreateRule(PropertyInfo source, PropertyInfo destiny, IHashCodeFactoryGenerator factoryGenerator);

    }
}
