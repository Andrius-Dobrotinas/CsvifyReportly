using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public interface ISingleCellValueEvaluator : IRowMatchEvaluator
    {

    }

    /// <summary>
    /// Returns true for values that match a specified value
    /// </summary>
    public class SingleCellValueEvaluator : ISingleCellValueEvaluator
    {
        private readonly int targetColumnIndex;
        private readonly IValueComparer valueComparer;

        public SingleCellValueEvaluator(
            int targetColumnIndex,
            IValueComparer valueComparer)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.valueComparer = valueComparer;
        }

        public bool IsMatch(string[] row)
        {
            var sourceValue = row[targetColumnIndex];

            return valueComparer.IsMatch(sourceValue);
        }
    }
}