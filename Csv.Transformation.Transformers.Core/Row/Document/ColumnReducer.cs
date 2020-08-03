using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Forms a new document that only consists of selected columns, all presented in the order specified
    /// </summary>
    public class ColumnReducer : IStructureTransformer
    {
        private readonly int[] targetColumnIndexes;
        private readonly ICellReducer cellReducer;

        public ColumnReducer(int[] targetColumnIndexes, ICellReducer cellReducer)
        {
            this.targetColumnIndexes = targetColumnIndexes ?? throw new ArgumentNullException(nameof(targetColumnIndexes));
            if (targetColumnIndexes.Any() == false)
                throw new ArgumentException("No columns have been specified", nameof(targetColumnIndexes));

            this.cellReducer = cellReducer ?? throw new ArgumentNullException(nameof(cellReducer));
        }

        public string[] Tramsform(string[] row)
        {
            return cellReducer.Reduce(row, targetColumnIndexes);
        }

        public string[] TransformHeader(string[] row)
        {
            return Tramsform(row);
        }
    }
}