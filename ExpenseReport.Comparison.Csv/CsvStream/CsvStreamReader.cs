using Andy.Csv.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    /// <summary>
    /// Reads and parses a CSV stream, and makes sure that all rows are of the same length
    /// </summary>
    public interface ICsvStreamReader
    {
        string[][] Read(Stream source);
    }

    public class CsvStreamReader : ICsvStreamReader
    {
        private readonly ICsvStreamParser csvStreamParser;

        public CsvStreamReader(ICsvStreamParser csvStreamParser)
        {
            this.csvStreamParser = csvStreamParser;
        }

        public string[][] Read(Stream source)
        {
            string[][] rows = csvStreamParser.Read(source);

            if (!rows.Any())
            {
                return rows;
            }

            var columnCount = rows.First().Length;

            if (!rows.All(row => row.Length == columnCount))
                throw new CsvValidationException("All rows in a CSV file must have an equal number of columns");

            return rows;
        }
    }
}