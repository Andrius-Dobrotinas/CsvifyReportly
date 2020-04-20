using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.ExpenseReport
{
    public class StatementEntry
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Details { get; set; }
    }
}