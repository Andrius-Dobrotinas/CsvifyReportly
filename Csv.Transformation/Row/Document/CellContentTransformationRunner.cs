using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Transforms the contents of cells of a given document with a given <see cref="ICellContentTransformer"/>.
    /// Neither document structure (ie column number and/or order), nor column names, are ever affected.
    /// </summary>
    public class CellContentTransformationRunner : ITransformationRunner<ICellContentTransformer>
    {
        private readonly IRowTransformationRunner<IRowTransformer> rowTransformationRunner;

        public CellContentTransformationRunner(IRowTransformationRunner<IRowTransformer> rowTransformationRunner)
        {
            this.rowTransformationRunner = rowTransformationRunner;
        }

        public CsvDocument Transform(CsvDocument document, ICellContentTransformer transformer)
        {
            string[][] rows = document.ContentRows.Any()
                ? TransformRows(document.ContentRows, transformer)
                : document.ContentRows;

            return new CsvDocument
            {
                HeaderCells = document.HeaderCells,
                ContentRows = rows
            };
        }

        private string[][] TransformRows(string[][] rows, ICellContentTransformer transformer)
        {
            int expectedColumnCount = rows[0].Length;
            return rowTransformationRunner.TransformRows(transformer, rows, expectedColumnCount);
        }
    }
}