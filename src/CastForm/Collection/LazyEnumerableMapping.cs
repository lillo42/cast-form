using System;
using System.Collections;
using System.Collections.Generic;

namespace CastForm.Collection
{
    /// <summary>
    /// Lazy mapping between 2 IEnumerable
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destiny type to create</typeparam>
    public class LazyEnumerableMapping<TSource, TDestiny> : IMap<IEnumerable<TSource>, IEnumerable<TDestiny>>
    {
        private readonly IMap<TSource, TDestiny> _map;
        public LazyEnumerableMapping(IMap<TSource, TDestiny> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        public IEnumerable<TDestiny> Map(IEnumerable<TSource> source) 
            => new MappingEnumerable(source.GetEnumerator(), _map);

        public struct MappingEnumerable : IEnumerable<TDestiny>, IEnumerator<TDestiny>
        {
            private readonly IEnumerator<TSource> _source;
            private readonly IMap<TSource, TDestiny> _map;

            public MappingEnumerable(IEnumerator<TSource> source, IMap<TSource, TDestiny> map)
            {
                _source = source ?? throw new ArgumentNullException(nameof(source));
                _map = map ?? throw new ArgumentNullException(nameof(map));
                Current = default;
            }

            public bool MoveNext()
            {
                var move = _source.MoveNext();

                if (move)
                {
                    Current = _map.Map(_source.Current);
                }
                else
                {
                    Current = default;
                }

                return move;
            }

            public void Reset()
            {
                _source.Reset();
            }

            public TDestiny Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose() 
                => _source.Dispose();
            public IEnumerator<TDestiny> GetEnumerator() 
                => this;

            IEnumerator IEnumerable.GetEnumerator() 
                => GetEnumerator();
        }
    }
}
