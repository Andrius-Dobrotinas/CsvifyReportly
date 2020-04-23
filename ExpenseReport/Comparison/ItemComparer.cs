using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public interface IItemComparer<in TItem1, in TItem2>
    {
        bool AreEqual(TItem1 transaction, TItem2 statementEntry);
    }

    public class ItemComparer : IItemComparer<TransactionDetails, StatementEntry>
    {
        private readonly IMerchantNameComparer merchantNameComparer;

        public ItemComparer(IMerchantNameComparer merchantNameComparer)
        {
            this.merchantNameComparer = merchantNameComparer;
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
            return merchantNameComparer.DoStatementDetailsReferToMerchant(statement.Details, transcation.Merchant, transcation.IsPayPal);
        }
    }
}