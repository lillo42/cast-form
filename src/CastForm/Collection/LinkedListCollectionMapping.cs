using System;
using System.Collections.Generic;

namespace CastForm.Collection
{
    public class LinkedListCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, ICollection<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        public LinkedListCollectionMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        public ICollection<TDestiny> Map(IEnumerable<TSource> source)
        {
            var collection = new LinkedList<TDestiny>();

            foreach (var item in source)
            {
                collection.AddLast(item == null ? default : _map.Map(item));
            }

            return collection;
        }
    }
}
