using System;
using System.Collections.Generic;
using System.Linq;

namespace Macli.Processing
{
    public class Sequence<T>
    {
        public bool IsRunning { get; set; }

        private readonly IEqualityComparer<T> comparer;
        private readonly List<Subsequence<T>> subsequences;
        private readonly List<Action<Subsequence<T>>> rules;

        public Sequence(IEqualityComparer<T> comparer)
        {
            this.comparer = comparer;
            subsequences = new List<Subsequence<T>>();
            rules = new List<Action<Subsequence<T>>>();
        }

        public bool Applies(T x, T y) => comparer.Equals(x, y);
        public void AddRule(Action<Subsequence<T>> rule) => rules.Add(rule);

        public void Continue(T item) => subsequences.LastOrDefault()?.Add(item);

        public void StartNew(T item)
        {
            IsRunning = true;
            subsequences.Add(new Subsequence<T>(item));
        }

        public void ApplyRules()
        {
            foreach (var action in rules)
            {
                foreach (var subsequence in subsequences)
                {
                    action(subsequence);
                }
            }
        }
    }
}
