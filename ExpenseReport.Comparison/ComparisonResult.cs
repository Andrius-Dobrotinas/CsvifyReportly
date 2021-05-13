using Andy.ExpenseReport.Comparison;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public class ComparisonResult<TTransaction1, TTransaction2>
    {
        public IList<IList<Tuple<TTransaction1, TTransaction2>>> MatchGroups { get; set; }
        public IList<TTransaction1> UnmatchedTransactions1 { get; set; }
        public IList<TTransaction2> UnmatchedTransactions2 { get; set; }
    }
}