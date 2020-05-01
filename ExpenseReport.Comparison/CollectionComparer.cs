using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison
{
    public interface ICollectionComparer<TTransaction1, TTransaction2>
    {
        public ComparisonResult<TTransaction1, TTransaction2> Compare(
            IList<TTransaction1> transactions1,
            IList<TTransaction2> transactions2);
    }

    public class CollectionComparer<TTransaction1, TTransaction2> : ICollectionComparer<TTransaction1, TTransaction2>
    {
        private readonly IMatchFinder<TTransaction1, TTransaction2> matcher;

        public CollectionComparer(
            IMatchFinder<TTransaction1, TTransaction2> matcher)
        {
            this.matcher = matcher;
        }

        public ComparisonResult<TTransaction1, TTransaction2> Compare(
            IList<TTransaction1> transactions1,
            IList<TTransaction2> transactions2)
        {
            var matches = matcher.GetMatches(transactions1, transactions2);

            var unmatchedTransactions1 = transactions1
                .Except(
                    matches.Select(x => x.Item1))
                .ToArray();

            var unmatchedTransactions2 = transactions2
                .Except(
                    matches.Select(x => x.Item2))
                .ToArray();

            return new ComparisonResult<TTransaction1, TTransaction2>
            {
                Matches = matches,
                UnmatchedTransactions1 = unmatchedTransactions1,
                UnmatchedTransactions2 = unmatchedTransactions2
            };
        }
    }
}