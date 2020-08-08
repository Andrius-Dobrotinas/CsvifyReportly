using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Performs any sort of non-filter transformation on a given document
    /// </summary>
    /// <typeparam name="TRowTransformer">Type of transformation</typeparam>
    public class RowTransformer<TRowTransformer> : DocumentTransformer<TRowTransformer>
        where TRowTransformer : IRowTransformer
    {
        public RowTransformer(
            IColumnMapBuilder columnMapBuilder,
            IRowTransformerFactory<TRowTransformer> factory,
            ITransformationRunner<TRowTransformer> transformerRunner,
            IResultReporter reporter)
            : base(columnMapBuilder, factory, transformerRunner, reporter)
        {
        }
    }
}