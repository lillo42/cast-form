using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CastForm.Collection
{
    /// <summary>
    /// Map <see cref="IEnumerable{T}"/> to <see cref="HashSet{T}"/>
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to create</typeparam>
    public class IAsyncEnumerableMapping<TSource, TDestiny> : IMap<IAsyncEnumerable<TSource>, IAsyncEnumerable<TDestiny>>
    {
        private readonly Counter _counter;
        private readonly IMap<TSource, TDestiny> _map;

        /// <summary>
        /// Initialize a new instance of <see cref="IAsyncEnumerableMapping{TSource,TDestiny}"/>
        /// </summary>
        /// <param name="map">The <see cref="IMap{TSource, TDestiny}"/> implementation to use when map <typeparamref name="TSource"/> to  <typeparamref name="TDestiny"/></param>
        /// <param name="counter"></param>
        public IAsyncEnumerableMapping(IMap<TSource, TDestiny> map, Counter counter)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _counter = counter ?? throw new ArgumentNullException(nameof(counter));
        }

        /// <summary>
        /// Execute Map
        /// </summary>
        /// <param name="source">object to be map</param>
        /// <returns>new instance of <see cref="IAsyncEnumerable{TDestiny}"/></returns>
        public IAsyncEnumerable<TDestiny> Map(IAsyncEnumerable<TSource> source) 
            => new AsyncEnumerator(_map, source.GetAsyncEnumerator(), _counter);

        private struct AsyncEnumerator : IAsyncEnumerable<TDestiny>, IAsyncEnumerator<TDestiny>
        {
            private readonly IAsyncEnumerator<TSource> _source;
            private readonly IMap<TSource, TDestiny> _map;
            private readonly Counter _counter;
            private CancellationToken _cancellation;

            public AsyncEnumerator(IMap<TSource, TDestiny> map, IAsyncEnumerator<TSource> source, Counter counter)
            {
                _map = map ?? throw new ArgumentNullException(nameof(map));
                _source = source ?? throw new ArgumentNullException(nameof(source));
                _counter = counter ?? throw new ArgumentNullException(nameof(counter));
                Current = default!;
            }

            public IAsyncEnumerator<TDestiny> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
            {
                _cancellation = cancellationToken;
                return this;
            }

            public ValueTask DisposeAsync() 
                => _source.DisposeAsync();

            public async ValueTask<bool> MoveNextAsync()
            {
                if (await _source.MoveNextAsync() && !_cancellation.IsCancellationRequested)
                {
                    try
                    {
                        Current = _map.Map(_source.Current);
                        return true;
                    }
                    finally
                    {
                        _counter.Clean();
                    }
                }

                Current = default!;
                return false;
            }

            public TDestiny Current { get; private set; }
        }
    }
}
