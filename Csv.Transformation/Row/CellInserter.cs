using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public interface ICellInserter<TValue>
    {
        TValue[] Insert(TValue[] source, int targetPosition, TValue value);
    }

    public class CellInserter<TValue> : ICellInserter<TValue>
    {
        private readonly IArrayElementInserter<TValue> elementInserter;

        public CellInserter(IArrayElementInserter<TValue> elementInserter)
        {
            this.elementInserter = elementInserter;
        }

        public TValue[] Insert(TValue[] source, int targetPosition, TValue value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int lastIndexPlusOne = source.Length;

            if (targetPosition > lastIndexPlusOne)
                throw new StructureException($"The row is too short to add an item at a specified position (row length: {source.Length} vs target index: {targetPosition}");

            return elementInserter.Insert(source, targetPosition, value);
        }
    }
}