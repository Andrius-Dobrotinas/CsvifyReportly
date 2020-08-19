using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public static class ResultAggregation
    {
        public static IEnumerable<string[]> GetDataRows(
            IEnumerable<Tuple<string[], string[]>> matches,
            IEnumerable<string[]> unmatchedTransactions1,
            IEnumerable<string[]> unmatchedTransactions2,
            string[] separatorColumns,
            string[] source1DummyRow,
            string[] source2DummyRow,
            string[] source1Colums,
            string[] source2Colums)
        {
            var contentRowPairs = GetDataRowPairs(
                matches,
                unmatchedTransactions1,
                unmatchedTransactions2,
                source1DummyRow,
                source2DummyRow);

            var headerRowPair = new Tuple<string[], string[]>(source1Colums, source2Colums);

            var allRowPairs = CombineHeaderAndContent(headerRowPair, contentRowPairs);

            return allRowPairs
                .Select(pair => JoinTwoRows(pair, separatorColumns))
                .ToArray();
        }

        private static IEnumerable<Tuple<string[], string[]>> CombineHeaderAndContent(
            Tuple<string[], string[]> headerRowPair,
            IEnumerable<Tuple<string[], string[]>> contentRowPairs)
        {
            yield return headerRowPair;
            foreach (var pair in contentRowPairs)
                yield return pair;
        }

        private static IEnumerable<Tuple<string[], string[]>> GetDataRowPairs(
            IEnumerable<Tuple<string[], string[]>> matches,
            IEnumerable<string[]> unmatchedTransactions1,
            IEnumerable<string[]> unmatchedTransactions2,
            string[] source1DummyRow,
            string[] source2DummyRow)
        {
            var unmatchedRows1 = GetUnmatchedSource1RowPairs(
                unmatchedTransactions1,
                source2DummyRow);

            var unmatchedRows2 = GetUnmatchedSource2RowPairs(
                unmatchedTransactions2,
                source1DummyRow);

            return matches
                .Concat(unmatchedRows1)
                .Concat(unmatchedRows2);
        }

        private static IEnumerable<Tuple<string[], string[]>> GetUnmatchedSource1RowPairs(
            IEnumerable<string[]> unmatchedStatementEntries,
            string[] blankSource2Row)
        {            
            return unmatchedStatementEntries.Select(
                row => new Tuple<string[], string[]>(
                    row,
                    blankSource2Row));
        }

        private static IEnumerable<Tuple<string[], string[]>> GetUnmatchedSource2RowPairs(
            IEnumerable<string[]> unmatchedTransactions,
            string[] blankSource1Row)
        {
            return unmatchedTransactions.Select(
                row => new Tuple<string[], string[]>(
                    blankSource1Row,
                    row));
        }

        private static string[] JoinTwoRows(Tuple<string[], string[]> rowPair, string[] separatorColumns)
        {
            return rowPair.Item1
                .Concat(separatorColumns)
                .Concat(rowPair.Item2)
                .ToArray();
        }
    }
}