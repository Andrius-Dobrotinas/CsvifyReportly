using Andy.Csv;
using Andy.Csv.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    /// <summary>
    /// Reads a CSV stream and makes sure that all rows are of the same length.
    /// Throws a <see cref="CsvValidationException"/> if they're not.
    /// </summary>
    public interface ISafeCsvStreamReader
    {
        string[][] Read(Stream source);
    }

    public class SafeCsvStreamReader : ISafeCsvStreamReader
    {
        private readonly ICsvStreamParser csvStreamParser;

        public SafeCsvStreamReader(ICsvStreamParser csvStreamParser)
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
                throw new StructureException("All rows in a CSV file must have an equal number of cells");

            return rows;
        }
    }
}