using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Transforms the structure of a document with a given <see cref="IStructureTransformer"/>.
    /// Actual cell contents are not affected.
    /// </summary>
    public class StructureTransformationRunner : ITransformationRunner<IStructureTransformer>
    {
        private readonly IRowTransformationRunner<IRowTransformer> rowTransformationRunner;

        public StructureTransformationRunner(IRowTransformationRunner<IRowTransformer> rowTransformationRunner)
        {
            this.rowTransformationRunner = rowTransformationRunner;
        }

        public CsvDocument Transform(CsvDocument document, IStructureTransformer transformer)
        {
            string[] columns = transformer.TransformHeader(document.HeaderCells);
            int expectedCellCount = columns.Length;

            string[][] rows = rowTransformationRunner.TransformRows(
                transformer,
                document.ContentRows,
                expectedCellCount);

            return new CsvDocument
            {
                HeaderCells = columns,
                ContentRows = rows
            };
        }
    }
}