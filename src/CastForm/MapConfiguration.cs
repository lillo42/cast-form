using System;
using System.Collections.Generic;
using CastForm.Impl;

namespace CastForm
{
    /// <summary>
    /// The maps configurations.
    /// </summary>
    public abstract class MapConfiguration
    {
        private readonly Dictionary<Type, IMappingConfiguration> _mapping = new();
        
        /// <summary>
        /// Create map for <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The map type</typeparam>
        /// <returns>The <see cref="IMappingConfiguration{TDestiny}"/>.</returns>
        protected IMappingConfiguration<T> CreateMap<T>()
        {
            if (!_mapping.ContainsKey(typeof(T)))
            {
                _mapping[typeof(T)] = new MappingConfiguration<T>();
            }

            return (IMappingConfiguration<T>) _mapping[typeof(T)];
        }
    }
}
