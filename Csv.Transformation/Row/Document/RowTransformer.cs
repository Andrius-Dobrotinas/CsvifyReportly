using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Performs any sort of non-filter transformation on a given document
    /// </summary>
    /// <typeparam name="TRowTransformer">Type of transformation</typeparam>
    public class RowTransformer<TRowTransformer> : IDocumentTransformer
        where TRowTransformer : IRowTransformer
    {
        private readonly IColumnMapBuilder columnMapBuilder;
        private readonly IRowTransformerFactory<TRowTransformer> factory;
        private readonly ITransformationRunner<TRowTransformer> transformerRunner;

        public RowTransformer(
            IColumnMapBuilder columnMapBuilder,
            IRowTransformerFactory<TRowTransformer> factory,
            ITransformationRunner<TRowTransformer> transformerRunner)
        {
            this.columnMapBuilder = columnMapBuilder;
            this.factory = factory;
            this.transformerRunner = transformerRunner;
        }

        public CsvDocument TransformRows(CsvDocument document)
        {
            var columnIndexes = columnMapBuilder.GetColumnIndexMap(document.ColumnNames);

            var actualTransformer = factory.Build(columnIndexes);

            return transformerRunner.Transform(document, actualTransformer);
        }
    }
}