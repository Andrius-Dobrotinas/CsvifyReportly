using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedSingleCellValueEvaluatorFactory
        : IDocumentTransformerFactory<ISingleCellValueEvaluator>
    {
        private readonly SingleCellValueEvaluatorFactory singleCellValueEvaluatorFactory;

        public InvertedSingleCellValueEvaluatorFactory(
            string name,
            SingleCellValueEvaluatorFactory singleCellValueEvaluatorFactory)
        {
            this.Name = name;
            this.singleCellValueEvaluatorFactory = singleCellValueEvaluatorFactory;
        }

        public string Name { get; }

        public ISingleCellValueEvaluator Build(IDictionary<string, int> columnIndexes)
        {
            ISingleCellValueEvaluator instance = singleCellValueEvaluatorFactory.Build(columnIndexes);

            return new InvertedSingleCellValueEvaluator(instance);
        }
    }
}