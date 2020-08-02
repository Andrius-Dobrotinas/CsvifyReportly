using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedSingleCellValueEvaluatorFactory
        : IDocumentTransformerFactory<SingleCellValueEvaluator>
    {
        private readonly string targetColumnName;
        private readonly string targetValue;

        public InvertedSingleCellValueEvaluatorFactory(
            string targetColumnName,
            string targetValue)
        {
            this.targetColumnName = targetColumnName;
            this.targetValue = targetValue;
        }

        public SingleCellValueEvaluator Build(IDictionary<string, int> columnIndexes)
        {
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new SingleCellValueEvaluator(
                targetColumnIndex,
                new InvertedValueComparer(
                    new StraightforwardValueComparer(targetValue)));
        }
    }
}