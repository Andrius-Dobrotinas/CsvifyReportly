using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class MerchantNameStartsWithComparer : IDetailsComparer
    {
        public bool AreEqual(string statementDetailsString, string name)
        {
            return statementDetailsString.StartsWith(name, StringComparison.OrdinalIgnoreCase);

        }
    }
}