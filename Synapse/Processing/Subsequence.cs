using System.Collections.Generic;
using System.Linq;

namespace Synapse.Processing
{
    public class Subsequence<T>
    {
        private readonly List<T> items;

        public T Last => items.LastOrDefault();
        public T First => items.FirstOrDefault();
        public List<T> Head => items.Count > 0 ? items.GetRange(0, items.Count - 1) : new List<T>();
        public List<T> Tail => items.Count > 1 ? items.GetRange(1, items.Count - 1) : new List<T>();

        public Subsequence(T item)
        {
            items = new List<T> { item };
        }

        public void Add(T item)
        {
            items.Add(item);
        }
    }
}