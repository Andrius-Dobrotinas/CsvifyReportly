using Andy.ExpenseReport.Comparison;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Verifier.Statement
{
    public interface IComparer<TItem1, TItem2>
    {
        public ComparisonResult Compare(
            IList<string[]> transactionRows,
            IList<string[]> statementRows);
    }

    public class Comparer<TItem1, TItem2> : IComparer<TItem1, TItem2>
        where TItem1: IComparisonItemWithSourceData
        where TItem2: IComparisonItemWithSourceData
    {
        private readonly ICollectionComparer<TItem1, TItem2> comparer;
        private readonly ICsvRowParser<TItem1> item1Parser;
        private readonly ICsvRowParser<TItem2> item2Parser;

        public Comparer(
            ICollectionComparer<TItem1, TItem2> comparer,
            ICsvRowParser<TItem1> item1Parser,
            ICsvRowParser<TItem2> item2Parser)
        {
            this.comparer = comparer;
            this.item1Parser = item1Parser;
            this.item2Parser = item2Parser;
        }

        public ComparisonResult Compare(
            IList<string[]> transactionRows,
            IList<string[]> statementRows)
        {
            var transactions = transactionRows.Select(item2Parser.Parse).ToArray();
            var statementEntries = statementRows.Select(item1Parser.Parse).ToArray();

            var result = comparer.Compare(
                    statementEntries,
                    transactions);

            var matchRows = result.Matches
                .Select(
                        x => new Tuple<string[], string[]>(
                            x.Item1.SourceData,
                            x.Item2.SourceData))
                .ToArray();

            var unmatchedStatementEntries = result.UnmatchedStatementEntries
                .Select(x => x.SourceData)
                .ToArray();

            var unmatchedTransactions = result.UnmatchedTransactions
                .Select(x => x.SourceData)
                .ToArray();

            return new ComparisonResult
            {
                Matches = matchRows,
                UnmatchedStatementEntries = unmatchedStatementEntries,
                UnmatchedTransactions = unmatchedTransactions
            };
        }
    }
}