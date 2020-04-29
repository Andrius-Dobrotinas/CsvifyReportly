using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public static class CsvStreamReader
    {
        public static string[][] Read(
            Stream source,
            char delimiter,
            out int columnCount)
        {
            string[][] rows = ReadRowsFromStream(source, delimiter);

            if (!rows.Any())
            {
                columnCount = 0;
                return rows;
            }

            // want to make sure all rows have equal number of columns. otherwise, things could get unpredictable down the line
            columnCount = rows.First().Length;

            int colCount = columnCount;
            if (!rows.All(row => row.Length == colCount))
                throw new CsvValidationException("All rows in a CSV file must have an equal number of columns");

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