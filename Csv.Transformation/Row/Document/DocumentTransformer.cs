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
        private readonly IResultReporter reporter;

        public DocumentTransformer(
            IColumnMapBuilder columnMapBuilder,
            IDocumentTransformerFactory<TTransformer> factory,
            ITransformationRunner<TTransformer> transformerRunner,
            IResultReporter reporter)
        {
            this.columnMapBuilder = columnMapBuilder;
            this.factory = factory;
            this.transformerRunner = transformerRunner;
            this.reporter = reporter;
        }

        public CsvDocument Transform(CsvDocument document)
        {
            var columnIndexes = columnMapBuilder.GetColumnIndexMap(document.HeaderCells);

            var actualTransformer = factory.Build(columnIndexes);

            reporter.ReportStart(factory.Name);

            var result = transformerRunner.Transform(document, actualTransformer);

            reporter.ReportFinish(document, result);

            return result;
        }
    }
}