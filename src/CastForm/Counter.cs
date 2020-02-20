using System.Collections.Concurrent;

namespace CastForm
{
    public class Counter
    {
        private readonly ConcurrentDictionary<int, int> _counter = new ConcurrentDictionary<int, int>();

        public int GetCounter(int hashCode)
        {
            _counter.TryGetValue(hashCode, out var counter);
            _counter[hashCode] = ++counter;
            return counter;
        }

        public void Clean(int hashCode) 
            => _counter.TryRemove(hashCode, out _);
    }
}
