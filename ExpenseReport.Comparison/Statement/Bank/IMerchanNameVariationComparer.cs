using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public interface IMerchanNameVariationComparer
    {
        bool IsMatch(string merchantName, string statementDetailsString);
    }
}