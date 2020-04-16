using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport
{
    public class ComparisonResult
    {
        public IList<Tuple<StatementEntry, TransactionDetails>> Matches { get; set; }
        public IList<TransactionDetails> UnmatchedTransactions { get; set; }
        public IList<StatementEntry> UnmatchedStatementEntries { get; set; }
    }
}