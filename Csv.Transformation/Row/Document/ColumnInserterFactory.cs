using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    public class ColumnInserterFactory : IRowTransformerFactory<ColumnInserter>
    {
        private readonly int targetColumnIndex;
        private readonly string targetColumnName;
        private readonly ICellInserter<string> cellInserter;

        public ColumnInserterFactory(
            int targetColumnIndex,
            string targetColumnName,
            ICellInserter<string> cellInserter)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.targetColumnName = targetColumnName;
            this.cellInserter = cellInserter;
        }

        public ColumnInserter Build(IDictionary<string, int> columnIndexes)
        {
            return new ColumnInserter(cellInserter, targetColumnIndex, targetColumnName);
        }
    }
}