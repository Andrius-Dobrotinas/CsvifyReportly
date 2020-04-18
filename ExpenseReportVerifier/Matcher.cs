using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.ExpenseReport
{
    public interface IMatcher
    {
        IList<Tuple<StatementEntry, TransactionDetails>> GetMatches(
            IEnumerable<StatementEntry> statement,
            IList<TransactionDetails> transactions);
    }

    public class Matcher : IMatcher
    {
        private readonly ITransactionAndStatementEntryComparer comparer;

        public Matcher(
            ITransactionAndStatementEntryComparer comparer)
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
