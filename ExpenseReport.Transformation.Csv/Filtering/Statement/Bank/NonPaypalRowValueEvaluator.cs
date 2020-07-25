using Andy.Csv.Transformation.Row.Filtering;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Transformation.Csv.Filtering.Statement.Bank
{
    /// <summary>
    /// Returns true for values that don't match a specified value
    /// </summary>
    public class NonPaypalRowValueEvaluator : IRowMatchEvaluator
    {
        private readonly int targetColumnIndex;
        private readonly IPaypalTransactionSpotter paypalTransactionSpotter;

        public NonPaypalRowValueEvaluator(
            int targetColumnIndex,
            IPaypalTransactionSpotter paypalTransactionSpotter)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.paypalTransactionSpotter = paypalTransactionSpotter;
        }

        public bool IsMatch(string[] row)
        {
            var sourceValue = row[targetColumnIndex];

            // todo: make this component more generic (so this and InvertedSingleRowValueEvaluated are one)

            return !paypalTransactionSpotter.IsPaypalTransaction(sourceValue);
        }
    }
}