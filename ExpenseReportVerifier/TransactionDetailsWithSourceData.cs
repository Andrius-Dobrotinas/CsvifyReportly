using Andy.ExpenseReport.Comparison;
using System;

namespace Andy.ExpenseReport.Cmd
{
    public class TransactionDetailsWithSourceData : TransactionDetails
    {
        public string[] SourceData { get; set; }
    }
}