using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CastForm
{
    public class Counter
    {
        private readonly ConcurrentDictionary<int, Dictionary<int, int>> _counter = new ConcurrentDictionary<int, Dictionary<int, int>>();

        public int GetCounter(int hashCode)
        {
            var counters = _counter.GetOrAdd(GenCurrentHashCode(), _ => new Dictionary<int, int>());
            counters.TryGetValue(hashCode, out var counter);
            counters[hashCode] = ++counter;

            return counter;
        }

        public void Clean()
        {
            if (_counter.TryRemove(GenCurrentHashCode(), out var clean))
            {
               clean.Clear();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GenCurrentHashCode()
            => HashCode.Combine(Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
    }
}
