using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public interface IMerchantNameComparer
    {
        bool DoStatementDetailsReferToMerchant(
            string statementDetails,
            string reportMerchant,
            bool isViaPayPal);
    }
}