using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public class ComparisonResult
    {
        public IList<Tuple<string[], string[]>> Matches { get; set; }
        public IList<Tuple<string[], string[]>> MatchesSecondary { get; set; }
        public IList<string[]> UnmatchedTransactions2 { get; set; }
        public IList<string[]> UnmatchedTransactions1 { get; set; }
    }
}