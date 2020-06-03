using System;
using System.Collections.Generic;

namespace CastForm.Collection
{
    /// <summary>
    /// Map <see cref="IEnumerable{T}"/> to <see cref="ICollection{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class ICollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, ICollection<TDestiny>>
    {
        private readonly Counter _counter;
        private readonly IMap<TSource, TDestiny> _map;

        /// <summary>
        /// Initialize a new instance of <see cref="ICollectionMapping{TSource, TDestiny}"/>
        /// </summary>
        /// <param name="map">The <see cref="IMap{TSource, TDestiny}"/> implementation to use when map <typeparamref name="TSource"/> to  <typeparamref name="TDestiny"/></param>
        /// <param name="counter"></param>
        public ICollectionMapping(IMap<TSource, TDestiny> map, Counter counter)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _counter = counter;
        }

        /// <inheritdoc/>
        public ICollection<TDestiny> Map(IEnumerable<TSource> source)
        {
            var collection = new LinkedList<TDestiny>();

            foreach (var item in source)
            {
                TDestiny destiny = default;

                if (item != null)
                {
                    try
                    {
                        destiny = _map.Map(item);
                    }
                    finally
                    {
                        _counter.Clean();
                    }
                }
                
                collection.AddLast(destiny);
            }

            return collection;
        }
    }
}
