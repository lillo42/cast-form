using System;
using System.Collections.Generic;

namespace CastForm.Collection
{
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
