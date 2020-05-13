using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public interface IComparer<TItem1, TItem2>
    {
        public ComparisonResult Compare(
            IList<string[]> transactionRows1,
            IList<string[]> transactionRows2);
    }

    public class Comparer<TTransaction1, TTransaction2> : IComparer<TTransaction1, TTransaction2>
        where TTransaction1: IComparisonItemWithSourceData
        where TTransaction2: IComparisonItemWithSourceData
    {
        private readonly ICollectionComparer<TTransaction1, TTransaction2> comparer;
        private readonly ICsvRowParser<TTransaction1> item1Parser;
        private readonly ICsvRowParser<TTransaction2> item2Parser;

        public Comparer(
            ICollectionComparer<TTransaction1, TTransaction2> comparer,
            ICsvRowParser<TTransaction1> item1Parser,
            ICsvRowParser<TTransaction2> item2Parser)
        {
            this.comparer = comparer;
            this.item1Parser = item1Parser;
            this.item2Parser = item2Parser;
        }

        public ComparisonResult Compare(
            IList<string[]> transactionRows1,
            IList<string[]> transactionRows2)
        {
            var transactions1 = new TTransaction1[transactionRows1.Count];
            for (int i = 0; i < transactionRows1.Count; i++)
            {
                try
                {
                    transactions1[i] = item1Parser.Parse(transactionRows1[i]);
                }
                catch (Statement.CsvValueParsingException e)
                {
                    throw new InputParsingException(i + 1, 1, e);
                }
            }

            var transactions2 = new TTransaction2[transactionRows2.Count];
            for (int i = 0; i < transactionRows2.Count; i++)
            {
                try
                {
                    transactions2[i] = item2Parser.Parse(transactionRows2[i]);
                }
                catch (Statement.CsvValueParsingException e)
                {
                    throw new InputParsingException(i + 1, 2, e);
                }
            }

            var result = comparer.Compare(transactions1, transactions2);

            var matchRows = result.Matches
                .Select(
                        x => new Tuple<string[], string[]>(
                            x.Item1.SourceData,
                            x.Item2.SourceData))
                .ToArray();

            var unmatchedTransactions1 = result.UnmatchedTransactions1
                .Select(x => x.SourceData)
                .ToArray();

            var unmatchedTransactions2 = result.UnmatchedTransactions2
                .Select(x => x.SourceData)
                .ToArray();

            return new ComparisonResult
            {
                Matches = matchRows,
                UnmatchedStatementEntries = unmatchedTransactions1,
                UnmatchedTransactions = unmatchedTransactions2
            };
        }
    }
}