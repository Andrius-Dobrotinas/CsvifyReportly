using Andy.ExpenseReport.Comparison.Statement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Filtering.Statement.Bank
{
    public class PayPalTransactionFilter<TStatementEntry> : IFilter<TStatementEntry>
        where TStatementEntry : StatementEntry
    {
        private readonly IPaypalTransactionSpotter paypalTransactionSpotter;

        public PayPalTransactionFilter(IPaypalTransactionSpotter paypalTransactionSpotter)
        {
            this.paypalTransactionSpotter = paypalTransactionSpotter;
        }

        public IEnumerable<TStatementEntry> FilterOut(IEnumerable<TStatementEntry> source)
        {
            return source.Where(
                transaction => !paypalTransactionSpotter.IsPaypalTransaction(transaction.Details));
        }
    }
}
