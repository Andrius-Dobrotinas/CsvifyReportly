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

        public CsvDocument Transform(CsvDocument document)
        {
            var columnIndexes = columnMapBuilder.GetColumnIndexMap(document.HeaderCells);

            var actualTransformer = factory.Build(columnIndexes);

            ReportStart(factory.Name);

            var result = transformerRunner.Transform(document, actualTransformer);

            ReportFinish(document, result);

            return result;
        }

        private void ReportStart(string transformerName)
        {
            Console.WriteLine(@$"Running transformer ""{transformerName}""");
        }

        private void ReportFinish(CsvDocument before, CsvDocument after)
        {
            Console.WriteLine("Transformer finished");

            if (after.ContentRows.Length != before.ContentRows.Length)
            {
                Console.WriteLine("The following rows have been filtered out:");

                foreach (var row in before.ContentRows.Except(after.ContentRows))
                    Console.WriteLine(Stringicize(row));
            }

            if (HaveHeaderCellsChanged(before.HeaderCells, after.HeaderCells))
            {
                Console.WriteLine("Columns have been added/removed. The document is now made up of these columns:");
                Console.WriteLine(Stringicize(after.HeaderCells));
            }
        }

        private bool HaveHeaderCellsChanged(string[] before, string[] after)
        {
            if (after.Length != before.Length)
                return true;

            for (int i = 0; i < before.Length; i++)
                if (before[i] != after[i])
                    return true;

            return false;
        }

        private string Stringicize(string[] row)
        {
            return string.Join('|', row);
        }
    }
}