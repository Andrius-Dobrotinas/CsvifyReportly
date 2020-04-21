using Andy.ExpenseReport.Comparison;
using System;

namespace Andy.ExpenseReport.Verifier
{
    public class TransactionDetailsWithSourceData : TransactionDetails
    {
        public string[] SourceData { get; set; }
    }
}