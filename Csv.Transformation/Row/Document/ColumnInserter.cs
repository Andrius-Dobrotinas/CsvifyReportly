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
            this.cellInserter = cellInserter ?? throw new ArgumentNullException(nameof(cellInserter));
            this.targetColumnIndex = targetColumnIndex;

            if (string.IsNullOrEmpty(targetColumnName)) throw new ArgumentException("Column name cannot be empty", nameof(targetColumnName));

            this.targetColumnName = targetColumnName;
        }

        public string[] Tramsform(string[] row)
        {
            return cellInserter.Insert(row, targetColumnIndex, null);
        }

        public string[] TransformHeader(string[] headerCells)
        {
            if (headerCells.Contains(targetColumnName))
                throw new NonUniqueColumnException(targetColumnName);

            return cellInserter.Insert(headerCells, targetColumnIndex, targetColumnName);
        }
    }
}