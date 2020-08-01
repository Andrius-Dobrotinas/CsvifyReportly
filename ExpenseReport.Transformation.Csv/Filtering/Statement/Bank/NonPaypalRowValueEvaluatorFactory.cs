using Andy.Csv.Transformation.Row;
using Andy.ExpenseReport.Transformation.Csv.Filtering.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Filtering.Statement.Bank

{
    public class NonPaypalRowValueEvaluatorFactory
        : IDocumentTransformerFactory<NonPaypalRowValueEvaluator>
    {
        private readonly string targetColumnName;
        private readonly IPaypalTransactionSpotter paypalTransactionSpotter;

        public NonPaypalRowValueEvaluatorFactory(
            string targetColumnName,
            IPaypalTransactionSpotter paypalTransactionSpotter)
        {
            this.targetColumnName = targetColumnName;
            this.paypalTransactionSpotter = paypalTransactionSpotter;
        }

        public NonPaypalRowValueEvaluator Build(IDictionary<string, int> columnIndexes)
        {
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new NonPaypalRowValueEvaluator(
                targetColumnIndex,
                paypalTransactionSpotter);
        }
    }
}