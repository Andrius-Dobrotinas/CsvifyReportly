using Andy.Csv.Transformation.Row;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Filtering.Statement.Bank

{
    public class NonPaypalRowValueEvaluatorFactory
        : IDocumentTransformerFactory<NonPaypalRowValueEvaluator>
    {
        private readonly int targetColumnIndex;
        private readonly IPaypalTransactionSpotter paypalTransactionSpotter;

        public NonPaypalRowValueEvaluatorFactory(
            int targetColumnIndex,
            IPaypalTransactionSpotter paypalTransactionSpotter)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.paypalTransactionSpotter = paypalTransactionSpotter;
        }

        public NonPaypalRowValueEvaluator Build(IDictionary<string, int> columnIndexes)
        {
            return new NonPaypalRowValueEvaluator(
                targetColumnIndex,
                paypalTransactionSpotter);
        }
    }
}