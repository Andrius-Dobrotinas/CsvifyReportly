using System;

namespace Andy.ExpenseReport
{
    public class TransactionDetailsWithSourceData : TransactionDetails
    {
        public string[] SourceData { get; set; }
    }
}