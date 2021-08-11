using System;
using System.Collections.Generic;
using CastForm.Exceptions;

namespace CastForm.Impl
{
    /// <summary>
    /// The implement of <see cref="IMappingConfiguration{TDestiny}"/>
    /// </summary>
    /// <typeparam name="TDestiny"></typeparam>
    public class MappingConfiguration<TDestiny> : IMappingConfiguration<TDestiny>
    {
        private readonly Dictionary<Type, IPropertiesMappingConfiguration> _configurations = new();
        
        /// <inheritdoc />
        public IMappingConfiguration<TDestiny> From<TSource>()
        {
            if (_configurations.ContainsKey(typeof(TSource)))
            {
                throw new DuplicatedMappingException(typeof(TDestiny), typeof(TSource));
            }

            _configurations[typeof(TSource)] = new PropertiesMappingConfiguration<TDestiny, TSource>();
            return this;
        }

        /// <inheritdoc />
        public IMappingConfiguration<TDestiny> From<TSource>(Action<IPropertiesMappingConfiguration<TDestiny, TSource>> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            
            if (_configurations.ContainsKey(typeof(TSource)))
            {
                throw new DuplicatedMappingException(typeof(TDestiny), typeof(TSource));
            }
            
            var propertiesMapping = new PropertiesMappingConfiguration<TDestiny, TSource>();
            configuration(propertiesMapping);

            _configurations[typeof(TSource)] = propertiesMapping;
            
            return this;
        }
    }
}
