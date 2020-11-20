using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class StraighforwardDetailsComparer : IDetailsComparer
    {
        public bool AreEqual(string details1, string details2)
        {
            return string.Equals(details1, details2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}