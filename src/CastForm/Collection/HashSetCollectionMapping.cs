using System;
using System.Collections.Generic;

namespace CastForm.Collection
{
    /// <summary>
    /// Map as HashSet
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class HashSetCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, HashSet<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        public HashSetCollectionMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

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
    /// Map as ISet
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class ISetCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, ISet<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        public ISetCollectionMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

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
