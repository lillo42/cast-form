using System;
using System.Collections.Generic;

namespace CastForm.Collection
{
    /// <summary>
    /// Map <see cref="IEnumerable{T}"/> to <see cref="HashSet{T}"/>
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class HashSetCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, HashSet<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        /// <summary>
        /// Initialize a new instance of <see cref="HashSetCollectionMapping{TSource, TDestiny}"/>
        /// </summary>
        /// <param name="map">The <see cref="IMap{TSource, TDestiny}"/> implementation to use when map <typeparamref name="TSource"/> to  <typeparamref name="TDestiny"/></param>
        public HashSetCollectionMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        /// <summary>
        /// Execute Map
        /// </summary>
        /// <param name="source">object to be map</param>
        /// <returns>new instance of <see cref="HashSet{TDestiny}"/></returns>
        public HashSet<TDestiny> Map(IEnumerable<TSource> source)
        {
            var collection = new HashSet<TDestiny>();

            foreach (var item in source)
            {
                collection.Add(item == null ? default : _map.Map(item));
            }

            return collection;
        }
    }

    /// <summary>
    /// Map <see cref="IEnumerable{T}"/> to <see cref="ISet{T}"/>
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class ISetCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, ISet<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        /// <summary>
        /// Initialize a new instance of <see cref="ISetCollectionMapping{TSource, TDestiny}"/>
        /// </summary>
        /// <param name="map">The <see cref="IMap{TSource, TDestiny}"/> implementation to use when map <typeparamref name="TSource"/> to  <typeparamref name="TDestiny"/></param>
        public ISetCollectionMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        /// <summary>
        /// Execute Map
        /// </summary>
        /// <param name="source">object to be map</param>
        /// <returns>new instance of <see cref="ISet{TDestiny}"/></returns>
        public ISet<TDestiny> Map(IEnumerable<TSource> source)
        {
            var collection = new HashSet<TDestiny>();

            foreach (var item in source)
            {
                collection.Add(item == null ? default : _map.Map(item));
            }

            return collection;
        }
    }
}
