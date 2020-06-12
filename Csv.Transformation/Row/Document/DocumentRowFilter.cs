using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class DocumentRowFilter : IDocumentTransformer
    {
        private readonly Filter.IRowMatchEvaluator rowMatchEvaluator;

        public DocumentRowFilter(Filter.IRowMatchEvaluator rowMatchEvaluator)
        {
            this.rowMatchEvaluator = rowMatchEvaluator;
        }

        public IEnumerable<string[]> TransformRows(IEnumerable<string[]> rows)
            => rows.Where(rowMatchEvaluator.IsMatch);
    }
}