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
        private readonly IMatchFinder<TTransaction1, TTransaction2> matcherSecondary;

        public CollectionComparer(
            IMatchFinder<TTransaction1, TTransaction2> matcher,
            IMatchFinder<TTransaction1, TTransaction2> matcherSecondary)
        {
            this.matcher = matcher;
            this.matcherSecondary = matcherSecondary;
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

            var secondaryMatches = matcherSecondary.GetMatches(unmatchedTransactions1, unmatchedTransactions2);

            unmatchedTransactions1 = unmatchedTransactions1
                .Except(
                    secondaryMatches.Select(x => x.Item1))
                .ToArray();

            unmatchedTransactions2 = unmatchedTransactions2
                .Except(
                    secondaryMatches.Select(x => x.Item2))
                .ToArray();

            return new ComparisonResult<TTransaction1, TTransaction2>
            {
                Matches = matches,
                MatchesSecondary = secondaryMatches,
                UnmatchedTransactions1 = unmatchedTransactions1,
                UnmatchedTransactions2 = unmatchedTransactions2
            };
        }
    }
}