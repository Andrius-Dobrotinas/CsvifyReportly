using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Performs any sort of transformation on a given document
    /// </summary>
    /// <typeparam name="TTransformer">Type of transformation</typeparam>
    public abstract class DocumentTransformer<TTransformer> : IDocumentTransformer
    {
        private readonly IColumnMapBuilder columnMapBuilder;
        private readonly IDocumentTransformerFactory<TTransformer> factory;
        private readonly ITransformationRunner<TTransformer> transformerRunner;

        public DocumentTransformer(
            IColumnMapBuilder columnMapBuilder,
            IDocumentTransformerFactory<TTransformer> factory,
            ITransformationRunner<TTransformer> transformerRunner)
        {
            this.columnMapBuilder = columnMapBuilder;
            this.factory = factory;
            this.transformerRunner = transformerRunner;
        }

        public CsvDocument TransformRows(CsvDocument document)
        {
            var columnIndexes = columnMapBuilder.GetColumnIndexMap(document.HeaderCells);

            var actualTransformer = factory.Build(columnIndexes);

            return transformerRunner.Transform(document, actualTransformer);
        }
    }
}