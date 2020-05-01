using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public class ItemComparer : IItemComparer<ExpenseReportEntry, StatementEntry>
    {
        private readonly IMerchantNameComparer merchantNameComparer;

        public ItemComparer(IMerchantNameComparer merchantNameComparer)
        {
            this.merchantNameComparer = merchantNameComparer;
        }

        public bool AreEqual(ExpenseReportEntry transaction, StatementEntry statementEntry)
        {
            return IsAmountEqual(transaction, statementEntry)
                && IsMerchantEqual(transaction, statementEntry);
        }

        private static bool IsAmountEqual(ExpenseReportEntry transactionDetails, StatementEntry statement)
        {
            return transactionDetails.Amount == statement.Amount;
        }

        private bool IsMerchantEqual(ExpenseReportEntry transcation, StatementEntry statement)
        {
            return merchantNameComparer.DoStatementDetailsReferToMerchant(statement.Details, transcation.Merchant, transcation.IsPayPal);
        }
    }
}