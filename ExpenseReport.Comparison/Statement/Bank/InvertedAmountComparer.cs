using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public class InvertedAmountComparer : IAmountComparer
    {
        public bool AreEqual(decimal amount1, decimal amount2)
        {
            return amount1 == amount2 * -1;
        }
    }
}