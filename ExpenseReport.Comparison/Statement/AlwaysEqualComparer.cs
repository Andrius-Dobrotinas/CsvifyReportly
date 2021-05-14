using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class AlwaysEqualComparer : IDetailsComparer
    {
        public bool AreEqual(string statementDetailsString, string name)
        {
            return true;
        }
    }
}