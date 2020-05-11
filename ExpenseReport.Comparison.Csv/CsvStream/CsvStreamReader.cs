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
            string[][] rows = Andy.Csv.IO.CsvStreamParser.ReadNParse(source, delimiter);

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
    }
}