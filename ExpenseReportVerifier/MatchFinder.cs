using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport
{
    public interface IMatchFinder
    {
        IList<Tuple<StatementEntry, TransactionDetails>> GetMatches(
            IEnumerable<StatementEntry> statement,
            IList<TransactionDetails> transactions);
    }

    public class MatchFinder : IMatchFinder
    {
        private readonly IItemComparer comparer;

        public MatchFinder(
            IItemComparer comparer)
        {
            this.comparer = comparer;
        }

        public IList<Tuple<StatementEntry, TransactionDetails>> GetMatches(
            IEnumerable<StatementEntry> statement,
            IList<TransactionDetails> transactions)
        {
            var matches = new List<Tuple<StatementEntry, TransactionDetails>>();

            foreach (var statementEntry in statement)
            {
                var transaction = GetFirstMatchingTransaction(
                    statementEntry,
                    transactions);

                if (transaction != null)
                    matches.Add(new Tuple<StatementEntry, TransactionDetails>(statementEntry, transaction));                
            }

            return matches;
        }

        private TransactionDetails GetFirstMatchingTransaction(
            StatementEntry statementEntry,
            IList<TransactionDetails> transactions)
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
