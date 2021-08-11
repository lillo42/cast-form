using System;
using System.Linq.Expressions;

namespace CastForm
{
    /// <summary>
    /// The properties map configurations.
    /// </summary>
    /// <typeparam name="TDestiny">The destiny type.</typeparam>
    /// <typeparam name="TSource">The source type.</typeparam>
    public interface IPropertiesMappingConfiguration<TDestiny, TSource> : IPropertiesMappingConfiguration
    {
        /// <summary>
        /// Map the property.
        /// </summary>
        /// <param name="source">The destiny property.</param>
        /// <returns>The <see cref="IPropertyMappingConfigurationAction{TDestiny,TSource}"/>.</returns>
        IPropertyMappingConfigurationAction<TDestiny, TSource> Map(Expression<Func<TDestiny, object>> source);
        
        /// <summary>
        /// The <see cref="Action{T}"/> to be executed before start the map. 
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/>.</param>
        /// <returns>The current <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>.</returns>
        IPropertiesMappingConfiguration<TDestiny, TSource> Before(Action<TDestiny> action);
        
        /// <summary>
        /// The <see cref="Action{T1,T2}"/> to be executed map end. 
        /// </summary>
        /// <param name="action">The <see cref="Action{T1,T2}"/>.</param>
        /// <returns>The current <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>.</returns>
        IPropertiesMappingConfiguration<TDestiny, TSource> After(Action<TDestiny, TSource> action);
    }
    
    public interface IPropertiesMappingConfiguration { }
}
