using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CastForm.Impl
{
    /// <summary>
    /// The implementation of <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/>
    /// </summary>
    /// <typeparam name="TDestiny">The destiny type.</typeparam>
    /// <typeparam name="TSource">The source type.</typeparam>
    public class PropertiesMappingConfiguration<TDestiny, TSource> : IPropertiesMappingConfiguration<TDestiny, TSource>
    {
        private readonly Dictionary<Expression<Func<TDestiny, object>>, IPropertyMappingConfigurationAction<TDestiny, TSource>> _propertyActions = new();
        
        /// <inheritdoc />
        public IPropertyMappingConfigurationAction<TDestiny, TSource> Map(Expression<Func<TDestiny, object>> source) 
            => _propertyActions[source] = new PropertyMappingConfigurationAction<TDestiny, TSource>(this);

        private Action<TDestiny>? _before;
        
        /// <inheritdoc />
        public IPropertiesMappingConfiguration<TDestiny, TSource> Before(Action<TDestiny> action)
        {
            _before = action;
            return this;
        }
        
        private Action<TDestiny, TSource>? _after;
        
        /// <inheritdoc />
        public IPropertiesMappingConfiguration<TDestiny, TSource> After(Action<TDestiny, TSource> action)
        {
            _after = action;
            return this;
        }
    }
}
