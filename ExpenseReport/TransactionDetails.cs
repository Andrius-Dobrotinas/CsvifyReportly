using System;

namespace Andy.ExpenseReport
{
    public class TransactionDetails
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool IsPayPal { get; set; }
        public string Merchant { get; set; }
    }
}