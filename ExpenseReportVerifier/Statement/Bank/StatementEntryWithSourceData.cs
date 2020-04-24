using Andy.ExpenseReport.Comparison;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Statement.Bank
{
    public class StatementEntryWithSourceData : StatementEntry, IComparisonItemWithSourceData
    {
        public string[] SourceData { get; set; }
    }
}