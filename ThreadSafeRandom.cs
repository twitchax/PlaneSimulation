using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace PlaneSimulation
{
    internal class ThreadSafeRandom
    {
        private ConcurrentDictionary<int, Random> _randoms = new ConcurrentDictionary<int, Random>();

        internal int Next(int maxValue)
        {
            return _randoms
                .GetOrAdd(Thread.CurrentThread.ManagedThreadId, id => new Random())
                .Next(maxValue);
        }
    }
}