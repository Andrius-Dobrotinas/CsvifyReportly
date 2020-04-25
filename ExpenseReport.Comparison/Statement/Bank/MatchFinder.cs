using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{    
    public class MatchFinder<TStatementEntry, TTransactionDetails>
        : IMatchFinder<TStatementEntry, TTransactionDetails>
        where TStatementEntry : StatementEntry
        where TTransactionDetails : TransactionDetails
    {
        private readonly IItemComparer<TTransactionDetails, TStatementEntry> comparer;

        public MatchFinder(
            IItemComparer<TTransactionDetails, TStatementEntry> comparer)
        {
            this.comparer = comparer;
        }

        public IList<Tuple<TStatementEntry, TTransactionDetails>> GetMatches(
            IList<TStatementEntry> statement,
            IList<TTransactionDetails> transactions)
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

        private TTransactionDetails GetFirstMatchingTransaction(
            TStatementEntry statementEntry,
            IList<TTransactionDetails> transactions)
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