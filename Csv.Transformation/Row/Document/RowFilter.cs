using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Filters out rows in a given document using a given match evaluator
    /// </summary>
    public class RowFilter : IDocumentTransformer
    {
        private readonly Filter.IRowMatchEvaluator rowMatchEvaluator;

        public RowFilter(Filter.IRowMatchEvaluator rowMatchEvaluator)
        {
            this.rowMatchEvaluator = rowMatchEvaluator;
        }

        //public IEnumerable<string[]> TransformRows(IEnumerable<string[]> rows)
        //    => rows.Where(rowMatchEvaluator.IsMatch);

        public CsvDocument TransformRows(CsvDocument document)
        {
            throw new NotImplementedException();
        }
    }
}