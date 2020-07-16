using System;
using System.Collections;
using System.Collections.Generic;

namespace Andy.Collections
{
    /// <summary>
    /// A facility for iterating over a collection and adding new items at the end.
    /// </summary>
    public class WritingEnumerator<TValue> : IEnumerator<TValue>
    {
        private const int initialPosition = -1;

        private readonly IList<TValue> store;

        public WritingEnumerator(IList<TValue> store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            if (store.IsReadOnly == true) throw new ArgumentException("The collection cannot be read-only", nameof(store));

            sizeZeroBased = store.Count - 1;
        }

        /* Both size and position are zero-based so they can be easily compared to each
         * other without having to add or subtract a number each time.
         * Keeping a track of size like that (instead of getting it from the store)
         * for performance considerations */
        private int currentPoseesh = initialPosition;
        private int sizeZeroBased = initialPosition;

        public TValue Current => store[currentPoseesh];
        object IEnumerator.Current => Current;

        /// <summary>
        /// Indicates whether there are any more items after the current one in this collection
        /// </summary>
        public bool IsEndOfTheLine => currentPoseesh == sizeZeroBased;

        public bool MoveNext()
        {
            currentPoseesh++;
            return IsEndOfTheLine;
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

        public void Dispose()
        {
            ResetPosition();
        }

        public void Reset()
        {
            //from the documentation, it's unclear what's this supposed to do
            throw new InvalidOperationException();
        }
    }
}