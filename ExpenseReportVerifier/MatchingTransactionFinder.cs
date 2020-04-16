using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport
{
    public interface IMatchingTransactionFinder
    {
        public TransactionDetails GetFirstMatchingTransaction(
            StatementEntry statementEntry,
            IList<TransactionDetails> transactions);
    }

    public class MatchingTransactionFinder : IMatchingTransactionFinder
    {
        private IMerchantComparer merchantComparer;

        public MatchingTransactionFinder(IMerchantComparer merchantComparer)
        {
            this.merchantComparer = merchantComparer;
        }

        public TransactionDetails GetFirstMatchingTransaction(
            StatementEntry statementEntry,
            IList<TransactionDetails> transactions)
        {
            for (int i = 0; i < transactions.Count; i++)
            {
                var transactionDetails = transactions[i];

                if (transactionDetails == null) continue;

                if (IsAmountEqual(transactionDetails, statementEntry)
                    && IsMerchantEqual(transactionDetails, statementEntry))
                {
                    return transactionDetails;
                }
            }

            return null;
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