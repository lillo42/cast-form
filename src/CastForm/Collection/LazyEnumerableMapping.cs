using System;
using System.Collections;
using System.Collections.Generic;

namespace CastForm.Collection
{
    /// <summary>
    /// Lazy mapping between 2 <see cref="IEnumerable{T}"/>
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destiny type to create</typeparam>
    public class LazyEnumerableMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, IEnumerable<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;

        /// <summary>
        /// Initialize a new instance of <see cref="LazyEnumerableMapping{TSource, TDestiny}"/>
        /// </summary>
        /// <param name="map">The <see cref="IMap{TSource, TDestiny}"/> implementation to use when map <typeparamref name="TSource"/> to  <typeparamref name="TDestiny"/></param>
        public LazyEnumerableMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        /// <inheritdoc/>
        public IEnumerable<TDestiny> Map(IEnumerable<TSource> source) 
            => new MappingEnumerable(source.GetEnumerator(), _map);

        /// <summary>
        /// Iterator to execute Map foreach item  in  <see cref="IEnumerable{T}"/>
        /// </summary>
        private struct MappingEnumerable : IEnumerable<TDestiny>, IEnumerator<TDestiny>
        {
            private readonly IEnumerator<TSource> _source;
            private readonly IMap<TSource, TDestiny> _map;

            /// <summary>
            /// Initialize a new instance of <see cref="MappingEnumerable"/>
            /// </summary>
            /// <param name="source"></param>
            /// <param name="map"></param>
            public MappingEnumerable(IEnumerator<TSource> source, IMap<TSource, TDestiny> map)
            {
                _source = source ?? throw new ArgumentNullException(nameof(source));
                _map = map ?? throw new ArgumentNullException(nameof(map));
                Current = default!;
            }

            
            /// <inheritdoc/>
            public bool MoveNext()
            {
                var move = _source.MoveNext();

                Current = move ? _map.Map(_source.Current) : default!;

                return move;
            }
            
            /// <inheritdoc/>
            public void Reset()
            {
                _source.Reset();
            }

            /// <inheritdoc/>
            public TDestiny Current { get; private set; }

            object? IEnumerator.Current => Current;
            
            /// <inheritdoc/>
            public void Dispose() 
                => _source.Dispose();

            /// <inheritdoc/>
            public IEnumerator<TDestiny> GetEnumerator() 
                => this;

            IEnumerator IEnumerable.GetEnumerator() 
                => GetEnumerator();
        }
    }
}
