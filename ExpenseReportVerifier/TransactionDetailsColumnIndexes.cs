using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier
{
    public class TransactionDetailsColumnIndexes
    {
        public int Date { get; set; }
        public int Amount { get; set; }
        public int IsPayPal { get; set; }
        public int Merchant { get; set; }
    }
}