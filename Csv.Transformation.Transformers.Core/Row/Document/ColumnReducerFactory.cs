using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class ColumnReducerFactory : IRowTransformerFactory<ColumnReducer>
    {
        private readonly string[] targetColumnNames;

        public ColumnReducerFactory(string[] targetColumnNames)
        {
            this.targetColumnNames = targetColumnNames;
        }

        public ColumnReducer Build(IDictionary<string, int> columnIndexes)
        {
            int[] targetColumnIndexes = targetColumnNames
                .Select(
                    name => Column.GetOrThrow(columnIndexes, name))
                .ToArray();

            return new ColumnReducer(
                targetColumnIndexes,
                new CellReducer());
        }
    }
}