using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedSingleRowValueEvaluatorFactory
        : IDocumentTransformerFactory<InvertedSingleRowValueEvaluator>
    {
        private readonly string targetColumnName;
        private readonly string targetValue;

        public InvertedSingleRowValueEvaluatorFactory(
            string targetColumnName,
            string targetValue)
        {
            this.targetColumnName = targetColumnName;
            this.targetValue = targetValue;
        }

        public InvertedSingleRowValueEvaluator Build(IDictionary<string, int> columnIndexes)
        {
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new InvertedSingleRowValueEvaluator(
                targetColumnIndex,
                targetValue);
        }
    }
}