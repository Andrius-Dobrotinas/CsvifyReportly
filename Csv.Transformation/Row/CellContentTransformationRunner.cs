using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Transforms a given row's cell contents (no structural changes) using a given content transformer
    /// </summary>
    public class CellContentTransformationRunner : IRowTransformationRunner<ICellContentTransformer>
    {
        public string[][] TransformRows(ICellContentTransformer transformer, string[][] rows)
        {
            if (!rows.Any()) return rows;

            var result = new string[rows.Length][];
            int targetCellCount = rows[0].Length;

            for (int i = 0; i < rows.Length; i++)
            {
                string[] row = transformer.Tramsform(rows[i]);

                if (row.Length != targetCellCount)
                    throw new CellCountMismatchException(targetCellCount, row.Length, i);

                result[i] = row;
            }

            return result;
        }
    }
}