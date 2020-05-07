using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison
{    
    public class MatchFinder<TTransaction1, TTransaction2> : IMatchFinder<TTransaction1, TTransaction2>
        where TTransaction2 : class
    {
        private readonly IItemComparer<TTransaction2, TTransaction1> comparer;

        public MatchFinder(
            IItemComparer<TTransaction2, TTransaction1> comparer)
        {
            this.comparer = comparer;
        }

        public IList<Tuple<TTransaction1, TTransaction2>> GetMatches(
            IEnumerable<TTransaction1> transactions1,
            IEnumerable<TTransaction2> transactions2)
        {
            var matches = new List<Tuple<TTransaction1, TTransaction2>>();

            var transactions2Copy = transactions2.ToArray();

            foreach (var transaction in transactions1)
            {
                var matchingTransaction = GetFirstMatchingTransaction(
                    transaction,
                    transactions2Copy);

                if (matchingTransaction != null)
                    matches.Add(new Tuple<TTransaction1, TTransaction2>(transaction, matchingTransaction));                
            }

            return matches;
        }

        private TTransaction2 GetFirstMatchingTransaction(
            TTransaction1 targetTransaction,
            IList<TTransaction2> transactions)
        {
            for (int i = 0; i < transactions.Count; i++)
            {
                var transaction = transactions[i];
                if (transaction == null) continue;

                if (comparer.AreEqual(transaction, targetTransaction))
                {
                    transactions[i] = null;
                    return transaction;
                }
            }

            return null;
        }
    }
}