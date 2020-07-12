using System;
using System.Collections.Generic;

namespace Andy.Collections
{
    public class EnumerableCopyManager<T>
    {
        private const int defaultPosition = -1;

        private readonly IList<T> store;

        /* Both size and position are zero-based so they can be easily compared to each
         * other without having to add or subtract a number each time.
         * Keeping a track of size like that (instead of getting it from the store)
         * for performance considerations */
        private int currentPoseesh = defaultPosition;
        private int sizeZeroBased = defaultPosition;

        public EnumerableCopyManager(IList<T> store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));

            if (store.Count > 0) throw new ArgumentException("The storage collection must be empty");
        }

        public T Current => store[currentPoseesh];
        public bool IsEndOfTheLine => currentPoseesh == sizeZeroBased;

        public void AdvancePosition()
        {
            currentPoseesh++;
        }

        public void Reset()
        {
            currentPoseesh = defaultPosition;
        }

        public void Add(T value)
        {
            currentPoseesh++;
            sizeZeroBased++;
            store.Add(value);
        }
    }
}

