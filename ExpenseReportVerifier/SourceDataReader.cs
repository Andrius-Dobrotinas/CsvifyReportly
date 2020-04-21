using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Cmd
{
    public static class SourceDataReader
    {
        public static SourceData ReadSourceData(
            FileInfo statementFile,
            FileInfo transactionsFile,
            StatementCsvFileSettings statementFileSettings,
            TransactionCsvFileSettings transactionDetailsFileSettings)
        {
            int statementColumnCount;
            var statementRows = ReadAndValidateRowsFromFile(
                    statementFile,
                    statementFileSettings.Delimiter,
                    out statementColumnCount);

            int transactionColumnCount;
            var transactionRows = ReadAndValidateRowsFromFile(
                    transactionsFile,
                    transactionDetailsFileSettings.Delimiter,
                    out transactionColumnCount);

            var transactionRowParser = new TransactionDetailsParser(transactionDetailsFileSettings.ColumnIndexes);
            var statementRowParser = new StatementEntryParser(statementFileSettings.ColumnIndexes);

            var transactions = transactionRows.Select(transactionRowParser.Parse);
            var statementEntries = statementRows.Select(statementRowParser.Parse);

            return new SourceData
            {
                Transactions = transactions.ToArray(),
                TransactionColumnCount = transactionColumnCount,
                StatementEntries = statementEntries.ToArray(),
                StatementColumnCount = statementColumnCount
            };
        }

        private static string[][] ReadAndValidateRowsFromFile(
            FileInfo statementFile,
            char delimiter,
            out int columnCount)
        {
            string[][] statementRows = ReadRowsFromCsvFile(
                statementFile,
                delimiter);

            if (!statementRows.Any()) throw new Exception("The has no CSV content");

            // want to make sure all rows have equal number of columns. otherwise, it could get tricky down the line
            columnCount = statementRows.First().Length;

            int colCount = columnCount;
            if (!statementRows.All(row => row.Length == colCount))
                throw new Exception("All rows in a CSV file must have the same number of columns");

            return statementRows;
        }

        private static string[][] ReadRowsFromCsvFile(FileInfo file, char delimiter)
        {
            return Csv.IO.CsvFileReader.Read(file, line => Csv.RowParser.Parse(line, delimiter));
        }
    }
}