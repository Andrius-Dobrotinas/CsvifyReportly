using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public interface IMatchFinder
    {
        IList<Tuple<TStatementEntry, TTransactionDetails>> GetMatches<TStatementEntry, TTransactionDetails>(
            IList<TStatementEntry> statement,
            IList<TTransactionDetails> transactions)
            where TStatementEntry : StatementEntry
            where TTransactionDetails : TransactionDetails;
    }

    public class MatchFinder : IMatchFinder
    {
        private readonly IItemComparer comparer;

        public MatchFinder(
            IItemComparer comparer)
        {
            this.comparer = comparer;
        }

        public IList<Tuple<TStatementEntry, TTransactionDetails>> GetMatches<TStatementEntry, TTransactionDetails>(
            IList<TStatementEntry> statement,
            IList<TTransactionDetails> transactions)
            where TStatementEntry : StatementEntry
            where TTransactionDetails : TransactionDetails
        {
            var matches = new List<Tuple<TStatementEntry, TTransactionDetails>>();

            foreach (var statementEntry in statement)
            {
                var transaction = GetFirstMatchingTransaction(
                    statementEntry,
                    transactions);

                if (transaction != null)
                    matches.Add(new Tuple<TStatementEntry, TTransactionDetails>(statementEntry, transaction));                
            }

            return matches;
        }

        private TTransactionDetails GetFirstMatchingTransaction<TStatementEntry, TTransactionDetails>(
            TStatementEntry statementEntry,
            IList<TTransactionDetails> transactions)
            where TStatementEntry : StatementEntry
            where TTransactionDetails : TransactionDetails
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
