using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedRowMatchEvaluatorFactory
        : IDocumentTransformerFactory<IRowMatchEvaluator>
    {
        private readonly IDocumentTransformerFactory<IRowMatchEvaluator> rowMatchEvaluatorFactory;

        public InvertedRowMatchEvaluatorFactory(
            string name,
            IDocumentTransformerFactory<IRowMatchEvaluator> rowMatchEvaluatorFactory)
        {
            this.Name = name;
            this.rowMatchEvaluatorFactory = rowMatchEvaluatorFactory;
        }

        public string Name { get; }

        public IRowMatchEvaluator Build(IDictionary<string, int> columnIndexes)
        {
            IRowMatchEvaluator instance = rowMatchEvaluatorFactory.Build(columnIndexes);

            return new InvertedRowMatchEvaluator(instance);
        }
    }
}