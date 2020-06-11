using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation
{
    /// <summary>
    /// Returns only the specified columns
    /// </summary>
    public class ColumnReducer : IRowTransformer
    {
        private readonly int[] targetColumnIndexes;

        public ColumnReducer(int[] targetColumnIndexes)
        {
            this.targetColumnIndexes = targetColumnIndexes;
        }

        public string[] Tramsform(string[] row)
        {
            int targetRowLength = targetColumnIndexes.Length;
            string[] result = new string[targetRowLength];

            if ((row.Length - 1) < targetColumnIndexes.Max())
                throw new Exception($"The row has less columns than the maximum target column index {targetColumnIndexes.Max()}");

            int currentColumn = 0;
            for (int i = 0; i < row.Length; i++)
            {
                if (targetColumnIndexes.Contains(i))
                    result[currentColumn++] = row[i];
            }

            return result;
        }
    }
}