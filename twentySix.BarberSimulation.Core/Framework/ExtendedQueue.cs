using System.Collections;
using System.Collections.Generic;

namespace twentySix.BarberSimulation.Core.Framework
{
    public class ExtendedQueue<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> _list = new();

        public int Count => _list.Count;

        public void Enqueue(T t) => _list.AddLast(t);

        public T Dequeue()
        {
            var result = _list.First.Value;
            _list.RemoveFirst();
            return result;
        }

        public T Peek() => _list.First.Value;

        public bool Remove(T t) => _list.Remove(t);

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}