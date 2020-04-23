using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison
{
    public interface ICollectionComparer<TItem1, TItem2>
    {
        public ComparisonResult<TItem1, TItem2> Compare(
            IList<TItem1> statement,
            IList<TItem2> transactions);
    }

    public class CollectionComparer<TItem1, TItem2> : ICollectionComparer<TItem1, TItem2>
    {
        private readonly IMatchFinder<TItem1, TItem2> matcher;

        public CollectionComparer(
            IMatchFinder<TItem1, TItem2> matcher)
        {
            this.matcher = matcher;
        }

        public ComparisonResult<TItem1, TItem2> Compare(
            IList<TItem1> statement,
            IList<TItem2> transactions)
        {
            var matches = matcher.GetMatches(statement, transactions);

            var unmatchedStatementEntries = statement
                .Except(
                    matches.Select(x => x.Item1))
                .ToArray();

            var unmatchedTransactions = transactions
                .Except(
                    matches.Select(x => x.Item2))
                .ToArray();

            return new ComparisonResult<TItem1, TItem2>
            {
                Matches = matches,
                UnmatchedStatementEntries = unmatchedStatementEntries,
                UnmatchedTransactions = unmatchedTransactions
            };
        }
    }
}