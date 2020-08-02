using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    /// <summary>
    /// Returns true for values that don't match a specified value
    /// </summary>
    public class InvertedSingleCellValueEvaluator : IRowMatchEvaluator
    {
        private readonly int targetColumnIndex;
        private readonly string targetValue;

        public InvertedSingleCellValueEvaluator(
            int targetColumnIndex,
            string targetValue)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.targetValue = targetValue;
        }

        public bool IsMatch(string[] row)
        {
            var sourceValue = row[targetColumnIndex];

            return sourceValue != targetValue;
        }
    }
}