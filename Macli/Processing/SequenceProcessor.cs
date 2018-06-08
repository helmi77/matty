using System;
using System.Collections.Generic;
using System.Linq;

namespace Macli.Processing
{
    public class SequenceProcessor<T>
    {
        public List<Action<T>> Rules { get; set; }
        public Dictionary<string, Sequence<T>> Sequences { get; set; }

        public SequenceProcessor()
        {
            Rules = new List<Action<T>>();
            Sequences = new Dictionary<string, Sequence<T>>();
        }

        public void AddRule(Action<T> rule)
        {
            Rules.Add(rule);
        }

        public void AddSequenceRule(string sequenceName, Action<Subsequence<T>> rule)
        {
            Sequences[sequenceName].AddRule(rule);
        }

        public void DefineSequence(string name, IEqualityComparer<T> comparer)
        {
            Sequences.Add(name, new Sequence<T>(comparer));
        }

        public void Process(List<T> items)
        {
            if (items.Count <= 1) return;
            T lastItem = items.First();
            Rules.ForEach(rule => rule(lastItem));

            foreach (Sequence<T> sequence in Sequences.Values)
                sequence.StartNew(lastItem);

            for (int i = 1; i < items.Count; i++)
            {
                T item = items[i];
                Rules.ForEach(rule => rule(item));
                foreach (Sequence<T> sequence in Sequences.Values)
                {
                    if (sequence.Applies(lastItem, item))
                    {
                        if (sequence.IsRunning)
                            sequence.Continue(item);
                        else
                            sequence.StartNew(item);
                    }
                    else
                        sequence.StartNew(item);
                }

                lastItem = item;
            }

            foreach (Sequence<T> sequence in Sequences.Values)
                sequence.ApplyRules();
        }
    }
}