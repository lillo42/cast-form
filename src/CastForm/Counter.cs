using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CastForm
{
    /// <summary>
    /// Counter how many time some type already be mapper
    /// </summary>
    public class Counter
    {
        private readonly ConcurrentDictionary<int, Dictionary<int, int>> _counter = new ConcurrentDictionary<int, Dictionary<int, int>>();

        /// <summary>
        /// Get Counter and increase it.
        /// </summary>
        /// <param name="hashCode"></param>
        /// <returns></returns>
        public int GetCounter(int hashCode)
        {
            var counters = _counter.GetOrAdd(GenCurrentHashCode(), _ => new Dictionary<int, int>());
            counters.TryGetValue(hashCode, out var counter);
            counters[hashCode] = ++counter;

            return counter;
        }

        /// <summary>
        /// Clean current counter.
        /// </summary>
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
