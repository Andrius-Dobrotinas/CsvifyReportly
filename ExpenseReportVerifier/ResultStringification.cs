using Andy.ExpenseReport.Comparison;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Cmd
{
    public class ResultStringification
    {
        public static string[] StringyfyyResults(
            ComparisonResult<StatementEntryWithSourceData, TransactionDetailsWithSourceData> result,
            int statementColumnCount,
            int transactionColumnCount,
            char csvDelimiter,
            Csv.IRowStringifier stringyfier)
        {
            var transactionAndStatementSeparatorColumns = new string[] { "" };
            var blankStatementRow = new string[statementColumnCount];
            var blankTransactionRow = new string[transactionColumnCount];

            var matchRows = result.Matches.Select(
                x => new Tuple<string[], string[]>(
                    x.Item1.SourceData,
                    x.Item2.SourceData));

            var allRows = ResultAggregation.GetDataRows(
                matchRows,
                result.UnmatchedStatementEntries.Select(x => x.SourceData),
                result.UnmatchedTransactions.Select(x => x.SourceData),
                transactionAndStatementSeparatorColumns,
                blankStatementRow,
                blankTransactionRow);

            return allRows
                .Select(row => stringyfier.Stringifififiify(row, csvDelimiter))
                .ToArray();
        }
    }
}
