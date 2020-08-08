using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    public class RowFilterer : DocumentTransformer<Filtering.IRowMatchEvaluator>
    {
        public RowFilterer(
            IColumnMapBuilder columnMapBuilder,
            IDocumentTransformerFactory<Filtering.IRowMatchEvaluator> factory,
            ITransformationRunner<Filtering.IRowMatchEvaluator> transformerRunner,
            IResultReporter reporter)
            : base(columnMapBuilder, factory, transformerRunner, reporter)
        {

        }
    }
}