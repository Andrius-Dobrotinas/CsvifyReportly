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
        private readonly IMatcher matcher;

        public TransactionAndStatementEntryMatcher(
            IMatcher matcher)
        {
            this.matcher = matcher;
        }

        public ComparisonResult CheckForMatches(
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