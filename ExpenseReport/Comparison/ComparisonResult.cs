using Andy.ExpenseReport.Comparison;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public class ComparisonResult<TItem1, TItem2>
    {
        public IList<Tuple<TItem1, TItem2>> Matches { get; set; }
        public IList<TItem2> UnmatchedTransactions { get; set; }
        public IList<TItem1> UnmatchedStatementEntries { get; set; }
    }
}