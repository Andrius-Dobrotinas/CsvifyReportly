using Andy.Csv.Transformation.Row;
using Andy.Csv.Transformation.Row.Filtering;
using Andy.ExpenseReport.Transformation.Csv.Filtering.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Filtering.Statement.Bank

{
    public class NonPaypalRowValueEvaluatorFactory
        : IDocumentTransformerFactory<SingleCellValueEvaluator>
    {
        private readonly string targetColumnName;
        private readonly IPaypalTransactionSpotter paypalTransactionSpotter;

        public NonPaypalRowValueEvaluatorFactory(
            string name,
            string targetColumnName,
            IPaypalTransactionSpotter paypalTransactionSpotter)
        {
            this.Name = name;
            this.targetColumnName = targetColumnName;
            this.paypalTransactionSpotter = paypalTransactionSpotter;
        }

        public string Name { get; }

        public SingleCellValueEvaluator Build(IDictionary<string, int> columnIndexes)
        {
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new SingleCellValueEvaluator(
                targetColumnIndex,
                new InvertedValueComparer(paypalTransactionSpotter));
        }
    }
}