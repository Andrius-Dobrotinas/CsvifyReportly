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
        private readonly ITransactionAndStatementEntryComparer comparer;

        public MatchingTransactionFinder(ITransactionAndStatementEntryComparer comparer)
        {
            this.comparer = comparer;
        }

        public TransactionDetails GetFirstMatchingTransaction(
            StatementEntry statementEntry,
            IList<TransactionDetails> transactions)
        {
            for (int i = 0; i < transactions.Count; i++)
            {
                var transactionDetails = transactions[i];

                if (transactionDetails == null) continue;

                if (comparer.AreEqual(transactionDetails, statementEntry))
                {
                    return transactionDetails;
                }
            }

            return null;
        }
    }
}