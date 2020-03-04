namespace twentySix.BarberSimulation.Core.Framework
{
    using System.Collections;
    using System.Collections.Generic;

    public class ExtendedQueue<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> list = new LinkedList<T>();

        public int Count => this.list.Count;

        public void Enqueue(T t)
        {
            this.list.AddLast(t);
        }

        public T Dequeue()
        {
            var result = this.list.First.Value;
            this.list.RemoveFirst();
            return result;
        }

        public T Peek()
        {
            return this.list.First.Value;
        }

        public bool Remove(T t)
        {
            return this.list.Remove(t);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}