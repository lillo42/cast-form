using System;
using System.Collections.Generic;

namespace CastForm.Collection
{
    /// <summary>
    /// Map as List
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class ListCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, List<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        public ListCollectionMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

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
    /// Map as IList
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class IListCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, IList<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        public IListCollectionMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

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
