using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Forms a new document that only consists of specified columns, all presented in a specified order
    /// </summary>
    public class ColumnReducer : ICellContentTransformer
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