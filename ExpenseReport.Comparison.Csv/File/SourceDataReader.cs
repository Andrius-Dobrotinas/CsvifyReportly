using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.File
{
    public static class SourceDataReader
    {
        public static SourceData ReadSourceData<TColumnIndexMap1, TColumnIndexMap2>(
            FileInfo statementFile,
            FileInfo transactionsFile,
            CsvFileParameters<TColumnIndexMap1> statementFileSettings,
            CsvFileParameters<TColumnIndexMap2> transactionDetailsFileSettings)
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

            return new SourceData
            {
                Transactions = transactionRows,
                TransactionColumnCount = transactionColumnCount,
                StatementEntries = statementRows,
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
            return Andy.Csv.IO.CsvFileReader.Read(file, line => Andy.Csv.RowParser.Parse(line, delimiter));
        }
    }
}