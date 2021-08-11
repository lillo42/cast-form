using System;
using System.Linq.Expressions;

namespace CastForm
{
    /// <summary>
    /// The action related with <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>.
    /// </summary>
    /// <typeparam name="TDestiny">The destiny type.</typeparam>
    /// <typeparam name="TSource">The source type.</typeparam>
    public interface IPropertyMappingConfigurationAction<TDestiny, TSource>
    {
        /// <summary>
        /// Ignore property.
        /// </summary>
        /// <returns>The original <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>.</returns>
        IPropertiesMappingConfiguration<TDestiny, TSource> Ignore();
        
        /// <summary>
        /// Select the <typeparamref name="TSource"/> property.
        /// </summary>
        /// <param name="source">The source property.</param>
        /// <returns>The original <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>.</returns>
        IPropertiesMappingConfiguration<TDestiny, TSource> From(Expression<Func<TSource, object>> source);
        
        /// <summary>
        /// From a constant value(resolved in compilation time).
        /// </summary>
        /// /// <param name="value">The constant value.</param>
        /// <returns>The original <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>.</returns>
        IPropertiesMappingConfiguration<TDestiny, TSource> FromConstant(Expression<Func<object>> value);
        
        /// <summary>
        /// From a constant value(resolved in compilation time).
        /// </summary>
        /// <param name="value">The constant value</param>
        /// <returns>The original <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>.</returns>
        IPropertiesMappingConfiguration<TDestiny, TSource> FromConstant(object value);
        
        /// <summary>
        /// From a value resolved in runtime. 
        /// </summary>
        /// <param name="value">The <see cref="Func{TResult}"/> to create value.</param>
        /// <returns>The original <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>.</returns>
        IPropertiesMappingConfiguration<TDestiny, TSource> FromValue(Func<object> value);
        
        /// <summary>
        /// From a value resolved in runtime. 
        /// </summary>
        /// <param name="value">The <see cref="Func{T, TResult}"/> to create value.</param>
        /// <returns>The original <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>.</returns>
        IPropertiesMappingConfiguration<TDestiny, TSource> FromValue(Func<TSource, object> value);
        
        
        /// <summary>
        /// The map when match with the conditional.
        /// </summary>
        /// <param name="conditional">The conditional</param>
        /// <returns>The current <see cref="IPropertyMappingConfigurationAction{TDestiny,TSource}"/>.</returns>
        IPropertyMappingConfigurationAction<TDestiny, TSource> When(Expression<Func<TSource, bool>> conditional);
        
        /// <summary>
        /// The map when the property is not null
        /// </summary>
        /// <param name="source">The property</param>
        /// <returns>The current <see cref="IPropertyMappingConfigurationAction{TDestiny,TSource}"/>.</returns>
        IPropertyMappingConfigurationAction<TDestiny, TSource> WhenNotNull(Expression<Func<TSource, object>> source);
    }
}
