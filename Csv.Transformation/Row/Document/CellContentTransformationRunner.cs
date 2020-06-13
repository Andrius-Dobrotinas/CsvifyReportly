using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Runs a transformation on a given document with a given <see cref="ICellContentTransformer"/>.
    /// No document structure (ie column number and/or order), nor column names, are ever affected.
    /// </summary>
    public class CellContentTransformationRunner : ITransformationRunner<ICellContentTransformer>
    {
        private readonly Row.IRowTransformationRunner<ICellContentTransformer> rowTransformationRunner;

        public CellContentTransformationRunner(Row.IRowTransformationRunner<ICellContentTransformer> rowTransformationRunner)
        {
            this.rowTransformationRunner = rowTransformationRunner;
        }

        public CsvDocument Transform(CsvDocument document, ICellContentTransformer transformer)
        {            
            string[][] rows = rowTransformationRunner.TransformRows(transformer, document.Rows);

            return new CsvDocument
            {
                ColumnNames = document.ColumnNames,
                Rows = rows
            };
        }
    }
}