using Andy.ExpenseReport.Comparison;
using Andy.ExpenseReport.Comparison.Statement.Bank;
using System;

namespace Andy.ExpenseReport.Verifier.Comparison
{
    public class TransactionDetailsWithSourceData : TransactionDetails
    {
        public string[] SourceData { get; set; }
    }
}