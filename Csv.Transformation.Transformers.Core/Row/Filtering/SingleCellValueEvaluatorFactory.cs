using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class SingleCellValueEvaluatorFactory
        : IDocumentTransformerFactory<IRowMatchEvaluator>
    {
        private readonly string targetColumnName;
        private readonly IValueComparer valueComparer;

        public SingleCellValueEvaluatorFactory(
            string name,
            string targetColumnName,
            IValueComparer valueComparer)
        {
            this.Name = name;
            this.targetColumnName = targetColumnName;
            this.valueComparer = valueComparer;
        }

        public string Name { get; }

        public IRowMatchEvaluator Build(IDictionary<string, int> columnIndexes)
        {
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new RowSingleCellValueEvaluator(
                targetColumnIndex,
                valueComparer);
        }
    }
}