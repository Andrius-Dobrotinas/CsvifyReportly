using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public interface IAmountComparer
    {
        bool AreEqual(decimal amount1, decimal amount2);
    }
}
