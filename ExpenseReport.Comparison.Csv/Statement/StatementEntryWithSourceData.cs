using Andy.ExpenseReport.Comparison.Statement;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement
{
    public class StatementEntryWithSourceData : StatementEntry, IComparisonItemWithSourceData
    {
        public string[] SourceData { get; set; }
    }
}