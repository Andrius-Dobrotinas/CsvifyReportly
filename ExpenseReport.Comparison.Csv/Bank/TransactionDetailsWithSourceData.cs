using Andy.ExpenseReport.Comparison.Statement.Bank;
using System;

namespace Andy.ExpenseReport.Comparison.Csv.Bank
{
    public class TransactionDetailsWithSourceData : TransactionDetails, IComparisonItemWithSourceData
    {
        public string[] SourceData { get; set; }
    }
}