using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport
{
    public interface ITransactionAndStatementEntryMatcher
    {
        public ComparisonResult CheckForMatches(
            IEnumerable<StatementEntry> statement,
            IList<TransactionDetails> transactions);
    }

    public class TransactionAndStatementEntryMatcher : ITransactionAndStatementEntryMatcher
    {
        private readonly IMatchingTransactionFinder matchingTransactionFinder;

        public TransactionAndStatementEntryMatcher(
            IMatchingTransactionFinder matchingTransactionFinder)
        {
            this.matchingTransactionFinder = matchingTransactionFinder;
        }

        public ComparisonResult CheckForMatches(
            IEnumerable<StatementEntry> statement,
            IList<TransactionDetails> transactions)
        {
            var matches = new List<Tuple<StatementEntry, TransactionDetails>>();
            var unmatchedStatementEntries = new List<StatementEntry>();

            foreach (var statementEntry in statement)
            {
                var transaction = matchingTransactionFinder.GetFirstMatchingTransaction(
                    statementEntry,
                    transactions);

                if (transaction != null)
                    matches.Add(new Tuple<StatementEntry, TransactionDetails>(statementEntry, transaction));
                else
                    unmatchedStatementEntries.Add(statementEntry);
            }

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