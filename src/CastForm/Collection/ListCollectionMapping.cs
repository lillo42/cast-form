using System;
using System.Collections.Generic;

namespace CastForm.Collection
{
    /// <summary>
    /// Map <see cref="IEnumerable{T}"/> to <see cref="List{T}"/>
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class ListCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, List<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        /// <summary>
        /// Initialize a new instance of <see cref="ListCollectionMapping{TSource, TDestiny}"/>
        /// </summary>
        /// <param name="map">The <see cref="IMap{TSource, TDestiny}"/> implementation to use when map <typeparamref name="TSource"/> to  <typeparamref name="TDestiny"/></param>
        public ListCollectionMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        /// <summary>
        /// Execute Map
        /// </summary>
        /// <param name="source">object to be map</param>
        /// <returns>new instance of <see cref="List{TDestiny}"/></returns>
        public List<TDestiny> Map(IEnumerable<TSource> source)
        {
            var collection = new List<TDestiny>();

            foreach (var item in source)
            {
                collection.Add(item == null ? default : _map.Map(item));
            }

            return collection;
        }
    }

    /// <summary>
    /// Map <see cref="IEnumerable{T}"/> to <see cref="IList{T}"/>
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class IListCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, IList<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        /// <summary>
        /// Initialize a new instance of <see cref="IListCollectionMapping{TSource, TDestiny}"/>
        /// </summary>
        /// <param name="map">The <see cref="IMap{TSource, TDestiny}"/> implementation to use when map <typeparamref name="TSource"/> to  <typeparamref name="TDestiny"/></param>
        public IListCollectionMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        /// <summary>
        /// Execute Map
        /// </summary>
        /// <param name="source">object to be map</param>
        /// <returns>new instance of <see cref="IList{TDestiny}"/></returns>
        public IList<TDestiny> Map(IEnumerable<TSource> source)
        {
            var collection = new List<TDestiny>();

            foreach (var item in source)
            {
                collection.Add(item == null ? default : _map.Map(item));
            }

            return collection;
        }
    }
}
