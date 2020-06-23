using Andy.Csv.Transformation.Row.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Filters out content rows in a given document using a given match evaluator
    /// </summary>
    public class RowFilterRunner : ITransformationRunner<IRowMatchEvaluator>
    {
        private readonly IRowFilter rowFilter;

        public RowFilterRunner(IRowFilter rowFilter)
        {
            this.rowFilter = rowFilter;
        }

        public CsvDocument Transform(CsvDocument document, IRowMatchEvaluator rowMatchEvaluator)
        {
            string[][] rows = rowFilter
                .Filter(document.ContentRows, rowMatchEvaluator)
                .ToArray();

            return new CsvDocument
            {
                HeaderCells = document.HeaderCells,
                ContentRows = rows,
            };
        }
    }
}