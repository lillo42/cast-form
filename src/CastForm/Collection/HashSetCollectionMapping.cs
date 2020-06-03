using System;
using System.Collections.Generic;

namespace CastForm.Collection
{
    /// <summary>
    /// Map <see cref="IEnumerable{T}"/> to <see cref="HashSet{T}"/>
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class HashSetCollectionMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, HashSet<TDestiny>>, IMap<IEnumerable<TSource>, ISet<TDestiny>>
    {
        private readonly Counter _counter;
        private readonly IMap<TSource, TDestiny> _map;

        /// <summary>
        /// Initialize a new instance of <see cref="HashSetCollectionMapping{TSource, TDestiny}"/>
        /// </summary>
        /// <param name="map">The <see cref="IMap{TSource, TDestiny}"/> implementation to use when map <typeparamref name="TSource"/> to  <typeparamref name="TDestiny"/></param>
        /// <param name="counter"></param>
        public HashSetCollectionMapping(IMap<TSource, TDestiny> map, Counter counter)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _counter = counter ?? throw new ArgumentNullException(nameof(counter));
        }

        /// <inheritdoc/>
        public HashSet<TDestiny> Map(IEnumerable<TSource> source)
        {
            var collection = new HashSet<TDestiny>();

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
                
                collection.Add(destiny);
            }

            return collection;
        }


        ISet<TDestiny> IMap<IEnumerable<TSource>, ISet<TDestiny>>.Map(IEnumerable<TSource> source)
        {
            return Map(source);
        }
    }
}
