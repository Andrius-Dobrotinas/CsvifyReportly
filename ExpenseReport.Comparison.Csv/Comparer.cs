﻿using System;
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
            var transactions1 = transactionRows1.Select(item1Parser.Parse).ToArray();
            var transactions2 = transactionRows2.Select(item2Parser.Parse).ToArray();

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