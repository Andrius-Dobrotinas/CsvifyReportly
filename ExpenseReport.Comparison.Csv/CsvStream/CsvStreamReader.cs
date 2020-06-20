using Andy.Csv.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public interface ICsvStreamReader
    {
        string[][] Read(Stream source, out int columnCount);
    }

    public class CsvStreamReader : ICsvStreamReader
    {
        private readonly ICsvStreamParser csvStreamParser;

        public CsvStreamReader(ICsvStreamParser csvStreamParser)
        {
            this.csvStreamParser = csvStreamParser;
        }

        public string[][] Read(Stream source, out int columnCount)
        {
            string[][] rows = csvStreamParser.Read(source);

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