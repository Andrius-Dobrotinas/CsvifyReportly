using System;
using System.Collections.Generic;

namespace Andy.Collections
{
    /// <summary>
    /// A facility for iterating over a collection and adding new items at the end.
    /// </summary>
    public class WritingEnumerator<TValue>
    {
        private const int initialPosition = -1;

        private readonly IList<TValue> store;

        public WritingEnumerator(IList<TValue> store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));

            if (store.Count > 0) throw new ArgumentException("The storage collection must be empty");
        }

        /* Both size and position are zero-based so they can be easily compared to each
         * other without having to add or subtract a number each time.
         * Keeping a track of size like that (instead of getting it from the store)
         * for performance considerations */
        private int currentPoseesh = initialPosition;
        private int sizeZeroBased = initialPosition;

        public TValue Current => store[currentPoseesh];

        /// <summary>
        /// Indicates whether there are any more items after the current one in this collection
        /// </summary>
        public bool IsEndOfTheLine => currentPoseesh == sizeZeroBased;

        public void AdvancePosition()
        {
            currentPoseesh++;
        }

        public void ResetPosition()
        {
            currentPoseesh = initialPosition;
        }

        /// <summary>
        /// Adds an item and advanced the current position so that the newly-added item is the current item
        /// </summary>
        /// <param name="value"></param>
        public void Add(TValue value)
        {
            currentPoseesh++;
            sizeZeroBased++;
            store.Add(value);
        }
    }
}