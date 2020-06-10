using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Filter
{
    /// <summary>
    /// Returns true for values that don't match a specified value
    /// </summary>
    public class InvertedSingleRowValueEvaluator : IRowMatchEvaluator
    {
        private readonly int targetColumnIndex;
        private readonly string targetValue;

        public InvertedSingleRowValueEvaluator(
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