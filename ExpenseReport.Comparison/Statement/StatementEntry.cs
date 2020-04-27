using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class StatementEntry
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Details { get; set; }
    }
}