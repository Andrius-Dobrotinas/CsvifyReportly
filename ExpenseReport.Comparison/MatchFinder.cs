using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{    
    public class MatchFinder<TItem1, TItem2>
        : IMatchFinder<TItem1, TItem2>
        where TItem2 : class
    {
        private readonly IItemComparer<TItem2, TItem1> comparer;

        public MatchFinder(
            IItemComparer<TItem2, TItem1> comparer)
        {
            this.comparer = comparer;
        }

        public IList<Tuple<TItem1, TItem2>> GetMatches(
            IList<TItem1> statement,
            IList<TItem2> transactions)
        {
            var matches = new List<Tuple<TItem1, TItem2>>();

            foreach (var statementEntry in statement)
            {
                var transaction = GetFirstMatchingTransaction(
                    statementEntry,
                    transactions);

                if (transaction != null)
                    matches.Add(new Tuple<TItem1, TItem2>(statementEntry, transaction));                
            }

            return matches;
        }

        private TItem2 GetFirstMatchingTransaction(
            TItem1 statementEntry,
            IList<TItem2> transactions)
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