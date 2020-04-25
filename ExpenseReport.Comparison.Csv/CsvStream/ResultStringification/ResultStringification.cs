using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public static class ResultStringification
    {
        public static string[] StringyfyyResults(
            ComparisonResult result,
            int statementColumnCount,
            int transactionColumnCount,
            char csvDelimiter,
            Andy.Csv.IRowStringifier stringyfier)
        {
            var transactionAndStatementSeparatorColumns = new string[] { "" };
            var blankStatementRow = new string[statementColumnCount];
            var blankTransactionRow = new string[transactionColumnCount];

            var allRows = ResultAggretation.GetDataRows(
                result.Matches,
                result.UnmatchedStatementEntries,
                result.UnmatchedTransactions,
                transactionAndStatementSeparatorColumns,
                blankStatementRow,
                blankTransactionRow);

            return allRows
                .Select(row => stringyfier.Stringifififiify(row, csvDelimiter))
                .ToArray();
        }
    }
}