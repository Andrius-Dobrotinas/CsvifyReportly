using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport
{
    public static class MatchingTransactionFinder
    {
        public static TransactionDetails GetFirstMatchingTransaction(
            StatementEntry statementEntry,
            IList<TransactionDetails> transactions,
            IItemComparer comparer)
        {
            for (int i = 0; i < transactions.Count; i++)
            {
                var transactionDetails = transactions[i];
                if (transactionDetails == null) continue;

                if (comparer.AreEqual(transactionDetails, statementEntry))
                {
                    transactions[i] = null;
                    return transactionDetails;
                }
            }

            return null;
        }
    }
}