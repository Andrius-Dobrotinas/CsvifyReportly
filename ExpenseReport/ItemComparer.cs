using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport
{
    public interface IItemComparer
    {
        bool AreEqual(TransactionDetails transaction, StatementEntry statementEntry);
    }

    public class ItemComparer : IItemComparer
    {
        private readonly IMerchantStringComparer merchantComparer;

        public ItemComparer(IMerchantStringComparer merchantComparer)
        {
            this.merchantComparer = merchantComparer;
        }

        public bool AreEqual(TransactionDetails transaction, StatementEntry statementEntry)
        {
            return IsAmountEqual(transaction, statementEntry)
                && IsMerchantEqual(transaction, statementEntry);
        }

        private static bool IsAmountEqual(TransactionDetails transactionDetails, StatementEntry statement)
        {
            return transactionDetails.Amount == statement.Amount;
        }

        private bool IsMerchantEqual(TransactionDetails transcation, StatementEntry statement)
        {
            return merchantComparer.DoStatementDetailsReferToMerchant(statement.Details, transcation.Merchant, transcation.IsPayPal);
        }
    }
}