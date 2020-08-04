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

            /* todo: maybe i should add a Name property to the transformer and have
             * that set by each factory? because the type name would be the same in
             * many cases. that would help identify specific configurations (each
             * config could have a name, by the way) */
            Console.WriteLine($"Running transformer {actualTransformer.GetType()}");

            var result = transformerRunner.Transform(document, actualTransformer);

            Console.WriteLine("Transformer finished");

            if (result.ContentRows.Length != document.ContentRows.Length)
            {
                Console.WriteLine("The following rows have been filtered out:");

                foreach (var row in document.ContentRows.Except(result.ContentRows))
                    Console.WriteLine(Stringicize(row));
            }

            if (result.HeaderCells.Length != document.HeaderCells.Length)
            {
                Console.WriteLine("Columns have been added/removed. The document is now made up of these columns:");
                Console.WriteLine(Stringicize(result.HeaderCells));

                // todo: even if the length hasn't change, check whether the names (or positions) have.
            }

            return result;
        }

        private string Stringicize(string[] row)
        {
            return string.Join('|', row);
        }
    }
}