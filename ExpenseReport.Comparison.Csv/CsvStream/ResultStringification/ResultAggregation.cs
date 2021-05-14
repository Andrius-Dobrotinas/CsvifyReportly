using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public static class ResultAggregation
    {
        public static IEnumerable<string[]> GetDataRows(
            IList<IList<Tuple<string[], string[]>>> matchGroups,
            IEnumerable<string[]> unmatchedTransactions1,
            IEnumerable<string[]> unmatchedTransactions2,
            int separatorColumnCount,
            string[] source1DummyRow,
            string[] source2DummyRow,
            string[] source1Colums,
            string[] source2Colums)
        {
            var unmatchedRows1 = GetUnmatchedSource1RowPairs(
                unmatchedTransactions1,
                source2DummyRow);

            var unmatchedRows2 = GetUnmatchedSource2RowPairs(
                unmatchedTransactions2,
                source1DummyRow);

            var separatorColumnsVanilla = new string[separatorColumnCount];
            var headerRowsFinal = JoinTwoRows(source1Colums, source2Colums, separatorColumnsVanilla);

            var matchGroupCount = matchGroups.Count;
            var matchGroupsFinal = new IEnumerable<string[]>[matchGroupCount];

            for (int i = 0; i < matchGroupCount; i++) {
                var separatorColumns = new string[separatorColumnCount];
                if (separatorColumns.Any())
                    separatorColumns[0] = i.ToString(); // Match level

                var matches = matchGroups[i];

                var final = matches.Select(pair => JoinTwoRows(pair, separatorColumns));
                matchGroupsFinal[i] = final;
            }

            var unmatchedRows1Final = unmatchedRows1
                .Select(pair => JoinTwoRows(pair, separatorColumnsVanilla));

            var unmatchedRows2Final = unmatchedRows2
                .Select(pair => JoinTwoRows(pair, separatorColumnsVanilla));

            return new string[][] { headerRowsFinal }
                .Concat(matchGroupsFinal.SelectMany(group => group)) // group.SelectMany(matches => matches)
                .Concat(unmatchedRows1Final)
                .Concat(unmatchedRows2Final)
                .ToArray();
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

        private static string[] JoinTwoRows(string[] rowLeft, string[] rowRight, string[] separatorColumns)
        {
            return rowLeft
                .Concat(separatorColumns)
                .Concat(rowRight)
                .ToArray();
        }
    }
}