using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public class ColumnInserterFactory : IRowTransformerFactory<ColumnInserter>
    {
        private readonly string targetColumnName;
        private readonly IArrayElementInserter<string> elementInserter;

        public ColumnInserterFactory(
            string targetColumnName,
            IArrayElementInserter<string> elementInserter)
        {
            this.targetColumnName = targetColumnName;
            this.elementInserter = elementInserter;
        }

        public ColumnInserter Build(IDictionary<string, int> columnIndexes)
        {
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new ColumnInserter(targetColumnIndex, elementInserter);
        }
    }
}