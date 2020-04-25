using Andy.ExpenseReport.Comparison.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Bank
{
    public class StatementEntryWithSourceData : StatementEntry, IComparisonItemWithSourceData
    {
        public string[] SourceData { get; set; }
    }
}