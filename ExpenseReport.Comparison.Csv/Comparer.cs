using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public interface IComparer<TEntry1, TEntry2>
    {
        ComparisonResult Compare(
            IList<string[]> transactionRows1,
            IList<string[]> transactionRows2);
    }

    public class Comparer<TTransaction1, TTransaction2> : IComparer<TTransaction1, TTransaction2>
        where TTransaction1: IHaveSourceData
        where TTransaction2: IHaveSourceData
    {
        private readonly ICollectionComparer<TTransaction1, TTransaction2> comparer;
        private readonly ICsvRowParser<TTransaction1> entry1Parser;
        private readonly ICsvRowParser<TTransaction2> entry2Parser;

        public Comparer(
            ICollectionComparer<TTransaction1, TTransaction2> comparer,
            ICsvRowParser<TTransaction1> entry1Parser,
            ICsvRowParser<TTransaction2> entry2Parser)
        {
            this.comparer = comparer;
            this.entry1Parser = entry1Parser;
            this.entry2Parser = entry2Parser;
        }

        public ComparisonResult Compare(
            IList<string[]> transactionRows1,
            IList<string[]> transactionRows2)
        {
            var transactions1 = ParseRows(transactionRows1, entry1Parser, 1);
            var transactions2 = ParseRows(transactionRows2, entry2Parser, 2);

            var result = comparer.Compare(transactions1, transactions2);

            var matchRows = result.MatchGroups
                .Select(CombineMatchedRowsIntoTuples)
                .ToList();
                
            var unmatchedTransactions1 = result.UnmatchedTransactions1
                .Select(x => x.SourceData)
                .ToArray();

            var unmatchedTransactions2 = result.UnmatchedTransactions2
                .Select(x => x.SourceData)
                .ToArray();

            return new ComparisonResult
            {
                Matches = matchRows,
                UnmatchedTransactions1 = unmatchedTransactions1,
                UnmatchedTransactions2 = unmatchedTransactions2
            };
        }

        private IList<Tuple<string[], string[]>> CombineMatchedRowsIntoTuples(IEnumerable<Tuple<TTransaction1, TTransaction2>> matches)
        {
            return matches
                .Select(
                        x => new Tuple<string[], string[]>(
                            x.Item1.SourceData,
                            x.Item2.SourceData))
                .ToArray();
        }

        private static TTransaction[] ParseRows<TTransaction>(
            IList<string[]> transactionRows,
            ICsvRowParser<TTransaction> rowParser,
            int sourceNumber)
        {
            var transactions = new TTransaction[transactionRows.Count];

            for (int i = 0; i < transactionRows.Count; i++)
            {
                try
                {
                    transactions[i] = rowParser.Parse(transactionRows[i]);
                }
                catch (Statement.CellValueParsingException e)
                {
                    throw new InputParsingException(i + 1, sourceNumber, e);
                }
            }

            return transactions;
        }
    }
}