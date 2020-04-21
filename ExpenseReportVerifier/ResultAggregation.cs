using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Verifier
{
    public static class ResultAggregation
    {
        public static IEnumerable<string[]> GetDataRows(
            IEnumerable<Tuple<string[], string[]>> matches,
            IEnumerable<string[]> unmatchedStatementEntries,
            IEnumerable<string[]> unmatchedTransactions,
            string[] separatorColumns,
            string[] blankStatementColumns,
            string[] blankTransactionColumns)
        {
            var allRowPairs = GetDataRowPairs(
                matches,
                unmatchedStatementEntries,
                unmatchedTransactions,
                blankStatementColumns,
                blankTransactionColumns);

            return allRowPairs
                .Select(pair => JoinTwoRows(pair, separatorColumns))
                .ToArray();
        }

        private static IEnumerable<Tuple<string[], string[]>> GetDataRowPairs(
            IEnumerable<Tuple<string[], string[]>> matches,
            IEnumerable<string[]> unmatchedStatementEntries,
            IEnumerable<string[]> unmatchedTransactions,
            string[] blankStatementColumns,
            string[] blankTransactionColumns)
        {
            var unmatchedStatementRows = GetUnmatchedStatementRowPairs(
                unmatchedStatementEntries,
                blankTransactionColumns);

            var unmatchedTransactionRows = GetUnmatchedTransactionRowPairs(
                unmatchedTransactions,
                blankStatementColumns);

            return matches
                .Concat(unmatchedStatementRows)
                .Concat(unmatchedTransactionRows);
        }

        private static IEnumerable<Tuple<string[], string[]>> GetUnmatchedStatementRowPairs(
            IEnumerable<string[]> unmatchedStatementEntries,
            string[] blankTransactionColumns)
        {            
            return unmatchedStatementEntries.Select(
                row => new Tuple<string[], string[]>(
                    row,
                    blankTransactionColumns));
        }

        private static IEnumerable<Tuple<string[], string[]>> GetUnmatchedTransactionRowPairs(
            IEnumerable<string[]> unmatchedTransactions,
            string[] blankStatementColumns)
        {
            return unmatchedTransactions.Select(
                row => new Tuple<string[], string[]>(
                    blankStatementColumns,
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