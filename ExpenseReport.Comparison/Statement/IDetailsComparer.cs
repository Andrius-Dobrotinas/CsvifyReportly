using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public interface IDetailsComparer
    {
        bool AreEqual(
            string details1,
            string details2);
    }
}