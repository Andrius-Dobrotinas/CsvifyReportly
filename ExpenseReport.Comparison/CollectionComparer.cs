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
        private readonly IList<IMatchFinder<TTransaction1, TTransaction2>> matchers;

        public CollectionComparer(IList<IMatchFinder<TTransaction1, TTransaction2>> matchers)
        {
            this.matchers = matchers;
        }

        public ComparisonResult<TTransaction1, TTransaction2> Compare(
            IList<TTransaction1> transactions1,
            IList<TTransaction2> transactions2)
        {
            var matchGroups = new List<IList<Tuple<TTransaction1, TTransaction2>>>(2);

            foreach (var matcher in matchers)
            {
                var (matches, unmatchedTransactions1, unmatchedTransactions2) = GetMatches(matcher, transactions1, transactions2);
                
                matchGroups.Add(matches);
                transactions1 = unmatchedTransactions1;
                transactions2 = unmatchedTransactions2;
            }

            return new ComparisonResult<TTransaction1, TTransaction2>
            {
                MatchGroups = matchGroups,
                UnmatchedTransactions1 = transactions1,
                UnmatchedTransactions2 = transactions2
            };
        }

        private (
            IList<Tuple<TTransaction1, TTransaction2>> matches, 
            TTransaction1[] unmatchedTransactions1, 
            TTransaction2[] unmatchedTransactions2) GetMatches(
            IMatchFinder<TTransaction1, TTransaction2> matcher,
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

            return (matches, unmatchedTransactions1, unmatchedTransactions2);
        }
    }
}