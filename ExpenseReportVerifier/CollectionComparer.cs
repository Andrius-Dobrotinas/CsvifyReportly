using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport
{
    public interface ICollectionComparer
    {
        public ComparisonResult Compare(
            IEnumerable<StatementEntry> statement,
            IList<TransactionDetails> transactions);
    }

    public class CollectionComparer : ICollectionComparer
    {
        private readonly IMatchFinder matcher;

        public CollectionComparer(
            IMatchFinder matcher)
        {
            this.matcher = matcher;
        }

        public ComparisonResult Compare(
            IEnumerable<StatementEntry> statement,
            IList<TransactionDetails> transactions)
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

            return new ComparisonResult
            {
                Matches = matches,
                UnmatchedStatementEntries = unmatchedStatementEntries,
                UnmatchedTransactions = unmatchedTransactions
            };
        }
    }
}