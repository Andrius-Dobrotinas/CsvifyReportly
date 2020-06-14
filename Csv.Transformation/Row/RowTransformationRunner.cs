using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Transforms a given row using a given transformer and making sure the resulting rows are of the expected length
    /// </summary>
    public class RowTransformationRunner : IRowTransformationRunner<IRowTransformer>
    {
        public string[][] TransformRows(
            IRowTransformer transformer,
            string[][] rows,
            int expectedCellCount)
        {
            if (!rows.Any()) return rows;

            var result = new string[rows.Length][];

            for (int i = 0; i < rows.Length; i++)
            {
                string[] row = transformer.Tramsform(rows[i]);

                if (row.Length != expectedCellCount)
                    throw new CellCountMismatchException(expectedCellCount, row.Length, i);

                result[i] = row;
            }

            return result;
        }
    }
}