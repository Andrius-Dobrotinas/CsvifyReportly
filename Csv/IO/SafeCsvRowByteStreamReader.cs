using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    /// <summary>
    /// Reads a CSV stream and makes sure that all rows are of the same length.
    /// Throws a <see cref="CsvValidationException"/> if they're not.
    /// </summary>
    public interface ISafeCsvRowByteStreamReader
    {
        string[][] Read(Stream source);
    }

    public class SafeCsvRowByteStreamReader : ISafeCsvRowByteStreamReader
    {
        private readonly ICsvRowByteStreamReader csvReader;

        public SafeCsvRowByteStreamReader(ICsvRowByteStreamReader csvReader)
        {
            this.csvReader = csvReader;
        }

        public string[][] Read(Stream source)
        {
            string[][] rows = csvReader.ReadRows(source).ToArray();

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