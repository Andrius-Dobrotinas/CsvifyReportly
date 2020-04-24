using Andy.ExpenseReport.Comparison;
using Andy.ExpenseReport.Comparison.Statement.Bank;
using System;

namespace Andy.ExpenseReport.Verifier.Statement.Bank
{
    public class TransactionDetailsWithSourceData : TransactionDetails, IComparisonItemWithSourceData
    {
        public string[] SourceData { get; set; }
    }
}