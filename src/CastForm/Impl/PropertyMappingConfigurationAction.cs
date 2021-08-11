using System;
using System.Linq.Expressions;

namespace CastForm.Impl
{
    /// <summary>
    /// The implementation of <see cref="IPropertyMappingConfigurationAction{TDestiny,TSource}"/>
    /// </summary>
    /// <typeparam name="TDestiny">The destiny type.</typeparam>
    /// <typeparam name="TSource">The source type.</typeparam>
    public class PropertyMappingConfigurationAction<TDestiny, TSource> : IPropertyMappingConfigurationAction<TDestiny, TSource>
    {
        private readonly IPropertiesMappingConfiguration<TDestiny, TSource> _origin;
        
        /// <summary>
        /// Initialize new instance of <see cref="PropertyMappingConfigurationAction{TDestiny,TSource}"/>.
        /// </summary>
        /// <param name="origin">The original <see cref="IPropertiesMappingConfiguration{TDestiny,TSource}"/></param>
        public PropertyMappingConfigurationAction(IPropertiesMappingConfiguration<TDestiny, TSource> origin)
        {
            _origin = origin;
        }

        /// <inheritdoc />
        public IPropertiesMappingConfiguration<TDestiny, TSource> Ignore() => _origin;

        /// <inheritdoc />
        public IPropertiesMappingConfiguration<TDestiny, TSource> From(Expression<Func<TSource, object>> source) => _origin;

        /// <inheritdoc />
        public IPropertiesMappingConfiguration<TDestiny, TSource> FromConstant(Expression<Func<object>> value) => _origin;

        private object? _fromConstant;
        /// <inheritdoc />
        public IPropertiesMappingConfiguration<TDestiny, TSource> FromConstant(object value)
        {
            _fromConstant = value;
            return _origin;
        }


        private Func<TSource, object>? _fromValue;
        /// <inheritdoc />
        public IPropertiesMappingConfiguration<TDestiny, TSource> FromValue(Func<object> value)
        {
            _fromValue = _ => value();
            return _origin;
        }
        
        /// <inheritdoc />
        public IPropertiesMappingConfiguration<TDestiny, TSource> FromValue(Func<TSource, object> value)
        {
            _fromValue = value;
            return _origin;
        }

        /// <inheritdoc />
        public IPropertyMappingConfigurationAction<TDestiny, TSource> When(Expression<Func<TSource, bool>> conditional) => this;

        /// <inheritdoc />
        public IPropertyMappingConfigurationAction<TDestiny, TSource> WhenNotNull(Expression<Func<TSource, object>> source) => this;
    }
}
