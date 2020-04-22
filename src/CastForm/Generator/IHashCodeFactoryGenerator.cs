using System;

namespace CastForm.Generator
{
    /// <summary>
    /// Create a class HashCodeFactory with methods calls GenHashCode with Type that was added.
    /// </summary>
    public interface IHashCodeFactoryGenerator
    {
        /// <summary>
        /// Type of HashCodeFactory.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Add override GenHashCode with type
        /// </summary>
        /// <param name="type">Type parameter that should be received in GenHashCode</param>
        void Add(Type type);

        /// <summary>
        /// Build class to create HashCodeFactory
        /// </summary>
        void Build();
        
        
    }
}
