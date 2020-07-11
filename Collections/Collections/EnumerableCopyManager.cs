using System;
using System.Collections.Generic;

namespace Andy.Collections
{
    public class EnumerableCopyManager<T>
    {
        private const int defaultPosition = -1;

        private readonly IList<T> store;

        private int poseesh = defaultPosition;
        private int size = defaultPosition;

        public EnumerableCopyManager(IList<T> store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));

            if (store.Count > 0) throw new ArgumentException("The storage collection must be empty");
        }

        public T Current => store[poseesh];
        public bool IsEndOfTheLine => poseesh == size;

        public void AdvancePosition()
        {
            poseesh++;
        }

        public void Reset()
        {
            poseesh = defaultPosition;
        }

        public void Add(T value)
        {
            poseesh++;
            size++;
            store.Add(value);
        }
    }
}