using System;

namespace CastForm
{
    /// <summary>
    /// The mapping configuration.
    /// </summary>
    /// <typeparam name="TDestiny">The destiny type.</typeparam>
    public interface IMappingConfiguration<TDestiny> : IMappingConfiguration
    {
        /// <summary>
        /// The map the <typeparamref name="TDestiny"/> from <typeparamref name="TSource"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <returns>The current <see cref="IMappingConfiguration{TDestiny}"/>.</returns>
        IMappingConfiguration<TDestiny> From<TSource>();
        
        /// <summary>
        /// The map the <typeparamref name="TDestiny"/> from <typeparamref name="TSource"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <param name="configuration">The mapping configuration</param>
        /// <returns>The current <see cref="IMappingConfiguration{TDestiny}"/>.</returns>
        IMappingConfiguration<TDestiny> From<TSource>(Action<IPropertiesMappingConfiguration<TDestiny, TSource>> configuration);
    }
    
    
    public interface IMappingConfiguration { }
}
