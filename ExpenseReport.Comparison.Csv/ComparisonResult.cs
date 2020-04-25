using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public class ComparisonResult
    {
        public IList<Tuple<string[], string[]>> Matches { get; set; }
        public IList<string[]> UnmatchedTransactions { get; set; }
        public IList<string[]> UnmatchedStatementEntries { get; set; }
    }
}