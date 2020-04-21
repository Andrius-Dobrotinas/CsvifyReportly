using Andy.ExpenseReport.Comparison;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public class ComparisonResult<TStatementEntry, TTransactionDetails>
        where TStatementEntry : StatementEntry
        where TTransactionDetails : TransactionDetails
    {
        public IList<Tuple<TStatementEntry, TTransactionDetails>> Matches { get; set; }
        public IList<TTransactionDetails> UnmatchedTransactions { get; set; }
        public IList<TStatementEntry> UnmatchedStatementEntries { get; set; }
    }
}