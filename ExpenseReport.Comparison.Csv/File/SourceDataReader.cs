using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.File
{
    public static class SourceDataReader
    {
        public static SourceData ReadSourceData<TColumnIndexMap1, TColumnIndexMap2>(
            Stream file1,
            Stream file2,
            CsvFileParameters<TColumnIndexMap1> csvSettings1,
            CsvFileParameters<TColumnIndexMap2> csvSettings2)
        {
            int statementColumnCount;
            var statementRows = ReadAndValidateRows(
                    file1,
                    csvSettings1.Delimiter,
                    out statementColumnCount);

            int transactionColumnCount;
            var transactionRows = ReadAndValidateRows(
                    file2,
                    csvSettings2.Delimiter,
                    out transactionColumnCount);            

            return new SourceData
            {
                Transactions = transactionRows,
                TransactionColumnCount = transactionColumnCount,
                StatementEntries = statementRows,
                StatementColumnCount = statementColumnCount
            };
        }

        private static string[][] ReadAndValidateRows(
            Stream source,
            char delimiter,
            out int columnCount)
        {
            string[][] rows = ReadRowsFromStream(
                source,
                delimiter);

            if (!rows.Any()) throw new Exception("The source has no CSV content");

            // want to make sure all rows have equal number of columns. otherwise, things could get unpredictable down the line
            columnCount = rows.First().Length;

            int colCount = columnCount;
            if (!rows.All(row => row.Length == colCount))
                throw new Exception("All rows in a CSV file must have an equal number of columns");

            return rows;
        }

        private static string[][] ReadRowsFromStream(Stream source, char delimiter)
        {
            return Andy.Csv.IO.CsvStreamReader.Read(
                source,
                line => Andy.Csv.RowParser.Parse(line, delimiter));
        }
    }
}