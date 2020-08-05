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
            string name,
            int targetColumnIndex,
            string targetColumnName,
            ICellInserter<string> cellInserter)
        {
            this.Name = name;
            this.targetColumnIndex = targetColumnIndex;
            this.targetColumnName = targetColumnName;
            this.cellInserter = cellInserter;
        }

        public string Name { get; }

        public ColumnInserter Build(IDictionary<string, int> columnIndexes)
        {
            return new ColumnInserter(cellInserter, targetColumnIndex, targetColumnName);
        }
    }
}