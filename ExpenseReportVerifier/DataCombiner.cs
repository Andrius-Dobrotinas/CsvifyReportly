using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport
{
    public static class DataCombiner
    {
        public static IEnumerable<string[]> GetDataRows(
            IList<Tuple<StatementEntryWithSourceData, TransactionDetailsWithSourceData>> matches,
            IList<StatementEntryWithSourceData> unmatchedStatementEntries,
            IList<TransactionDetailsWithSourceData> unmatchedTransactions,
            string[] separatorColumns,
            string[] blankStatementRow,
            string[] blankTransactionRow)
        {
            var allRowPairs = GetDataRowPairs(
                matches,
                unmatchedStatementEntries,
                unmatchedTransactions,
                blankStatementRow,
                blankTransactionRow);

            return allRowPairs
                .Select(pair => JoinTwoRows(pair, separatorColumns))
                .ToArray();
        }

        private static IEnumerable<Tuple<string[], string[]>> GetDataRowPairs(
            IList<Tuple<StatementEntryWithSourceData, TransactionDetailsWithSourceData>> matches,
            IList<StatementEntryWithSourceData> unmatchedStatementEntries,
            IList<TransactionDetailsWithSourceData> unmatchedTransactions,
            string[] blankStatementRow,
            string[] blankTransactionRow)
        {
            var matchRows = matches.Select(
                x => new Tuple<string[], string[]>(
                    x.Item1.SourceData,
                    x.Item2.SourceData));

            var unmatchedStatementRows = unmatchedStatementEntries.Select(
                row => new Tuple<string[], string[]>(
                    row.SourceData,
                    blankTransactionRow));

            var unmatchedTransactionRows = unmatchedTransactions.Select(
                row => new Tuple<string[], string[]>(
                    blankStatementRow,
                    row.SourceData));

            return matchRows
                .Concat(unmatchedStatementRows)
                .Concat(unmatchedTransactionRows);
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