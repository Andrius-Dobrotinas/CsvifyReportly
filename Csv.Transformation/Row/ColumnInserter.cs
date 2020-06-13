using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Inserts new empty columns into a document
    /// </summary>
    public class ColumnInserter : IStructureTransformer
    {
        private readonly int targetColumnIndex;
        private readonly string targetColumnName;// todo
        private readonly IArrayElementInserter<string> elementInserter;

        public ColumnInserter(
            int targetColumnIndex,
            IArrayElementInserter<string> elementInserter)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.elementInserter = elementInserter;
        }

        public string[] Tramsform(string[] source)
        {
            int lastIndexPlusOne = source.Length;

            if (targetColumnIndex > lastIndexPlusOne)
                throw new Exception($"The row is too short to add an item at a specified position (row length: {source.Length} vs target index: {targetColumnIndex}");

            return elementInserter.Insert(source, targetColumnIndex, null);
        }
    }
}