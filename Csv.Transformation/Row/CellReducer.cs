using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    public interface ICellReducer
    {
        string[] Reduce(string[] cells, int[] targetColumnIndexes);
    }

    public class CellReducer : ICellReducer
    {
        public string[] Reduce(string[] cells, int[] targetCellIndexes)
        {
            if (cells == null) throw new ArgumentNullException(nameof(cells));
            if (targetCellIndexes == null) throw new ArgumentNullException(nameof(targetCellIndexes));

            int targetRowLength = targetCellIndexes.Length;
            string[] result = new string[targetRowLength];

            if ((cells.Length - 1) < targetCellIndexes.Max())
                throw new ArgumentOutOfRangeException(
                    "At least one of target column indexes is greater than the index of the last cell in the row",
                    nameof(targetCellIndexes));

            int currentCellIndex = 0;
            for (int i = 0; i < cells.Length; i++)
            {
                if (targetCellIndexes.Contains(i))
                    result[currentCellIndex++] = cells[i];
            }

            return result;
        }
    }
}