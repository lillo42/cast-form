using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CastForm
{
    public class Counter
    {
        private readonly ConcurrentDictionary<int, int> _counter = new ConcurrentDictionary<int, int>();
        private readonly ConcurrentDictionary<int, ConcurrentQueue<int>> _counterToClean = new ConcurrentDictionary<int, ConcurrentQueue<int>>();

        public int GetCounter(int hashCode)
        {
            _counter.TryGetValue(hashCode, out var counter);
            _counter[hashCode] = ++counter;

            var toClean = HashCode.Combine(Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
            if (!_counterToClean.TryGetValue(toClean, out var link))
            {
                link = new ConcurrentQueue<int>();
                _counterToClean.TryAdd(toClean, link);
            }

            link.Enqueue(hashCode);
            return counter;
        }

        public void Clean(int hashToClean)
        {
            if (_counterToClean.TryRemove(hashToClean, out var clean))
            {
                foreach (var remove in clean)
                {
                    _counter.TryRemove(remove, out _);
                }
            }
        }
    }
}
