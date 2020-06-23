using System;
using System.Collections.Generic;

namespace Andy.Csv
{
    public interface IArrayElementInserter<T>
    {
        /// <summary>
        /// Produces a new array with a given value inserted at a specified position
        /// </summary>
        T[] Insert(T[] source, int targetPosition, T value);
    }

    public class ArrayElementInserter<T> : IArrayElementInserter<T>
    {
        public T[] Insert(T[] source, int targetPosition, T value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            T[] result = new T[source.Length + 1];

            int slice1Length = targetPosition;
            int origSlice2Start = targetPosition;

            var span = new Span<T>(source);
            var origSlice1 = span.Slice(0, slice1Length);
            var origSlice2 = span.Slice(origSlice2Start);

            var newSlice1 = result.AsSpan(0, slice1Length);
            var newSlice2 = result.AsSpan(origSlice2Start + 1);

            origSlice1.CopyTo(newSlice1);
            origSlice2.CopyTo(newSlice2);

            result[targetPosition] = value;

            return result;
        }
    }
}