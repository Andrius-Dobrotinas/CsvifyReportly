using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class AmountComparer : IAmountComparer
    {
        public bool AreEqual(decimal amount1, decimal amount2)
        {
            return amount1 == amount2;
        }
    }
}