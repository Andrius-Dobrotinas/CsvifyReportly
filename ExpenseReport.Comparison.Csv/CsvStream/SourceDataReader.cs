using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public static class SourceDataReader
    {
        public static SourceData ReadSourceData(
            Stream source1,
            Stream source2,
            char source1ValueDelimiter,
            char source2ValueDelimiter)
        {
            int statementColumnCount;
            var statementRows = ReadAndValidateRows(
                    source1,
                    source1ValueDelimiter,
                    out statementColumnCount);

            int transactionColumnCount;
            var transactionRows = ReadAndValidateRows(
                    source2,
                    source2ValueDelimiter,
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