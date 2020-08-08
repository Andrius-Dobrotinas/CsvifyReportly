using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class SingleCellValueEvaluatorFactory
        : IDocumentTransformerFactory<SingleCellValueEvaluator>
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

        public SingleCellValueEvaluator Build(IDictionary<string, int> columnIndexes)
        {
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new SingleCellValueEvaluator(
                targetColumnIndex,
                valueComparer);
        }
    }
}