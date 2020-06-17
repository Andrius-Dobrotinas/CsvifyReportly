using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Inserts new empty columns into a document
    /// </summary>
    public class ColumnInserter : IStructureTransformer
    {
        private readonly int targetColumnIndex;
        private readonly string targetColumnName;
        private readonly ICellInserter<string> cellInserter;

        public ColumnInserter(
            ICellInserter<string> cellInserter,
            int targetColumnIndex,
            string targetColumnName)
        {
            this.cellInserter = cellInserter;
            this.targetColumnIndex = targetColumnIndex;
            this.targetColumnName = targetColumnName;
        }

        public string[] Tramsform(string[] row)
        {
            return cellInserter.Insert(row, targetColumnIndex, null);
        }

        public string[] TransformHeader(string[] row)
        {
            return cellInserter.Insert(row, targetColumnIndex, targetColumnName);
        }
    }
}