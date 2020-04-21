using Andy.ExpenseReport.Comparison;
using System;

namespace Andy.ExpenseReport.Verifier.Comparison
{
    public class TransactionDetailsWithSourceData : TransactionDetails
    {
        public string[] SourceData { get; set; }
    }
}