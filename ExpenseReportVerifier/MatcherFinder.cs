using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.ExpenseReport
{
    public interface IMatcherFinder
    {
        IList<Tuple<StatementEntry, TransactionDetails>> GetMatches(
            IEnumerable<StatementEntry> statement,
            IList<TransactionDetails> transactions);
    }

    public class MatcherFinder : IMatcherFinder
    {
        private readonly IItemComparer comparer;

        public MatcherFinder(
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
                var transaction = MatchingTransactionFinder.GetFirstMatchingTransaction(
                    statementEntry,
                    transactions,
                    comparer);

                if (transaction != null)
                    matches.Add(new Tuple<StatementEntry, TransactionDetails>(statementEntry, transaction));                
            }

            return matches;
        }
    }
}
