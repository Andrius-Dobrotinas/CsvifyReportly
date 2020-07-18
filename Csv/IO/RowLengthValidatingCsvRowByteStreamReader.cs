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
    public interface IRowLengthValidatingCsvRowByteStreamReader
    {
        IEnumerable<string[]> Read(Stream source);
    }

    public class RowLengthValidatingCsvRowByteStreamReader : IRowLengthValidatingCsvRowByteStreamReader
    {
        private readonly ICsvRowByteStreamReader csvReader;

        public RowLengthValidatingCsvRowByteStreamReader(ICsvRowByteStreamReader csvReader)
        {
            this.csvReader = csvReader;
        }

        public IEnumerable<string[]> Read(Stream source)
        {
            IEnumerable<string[]> rows = csvReader.ReadRows(source);

            if (!rows.Any())
            {
                return rows;
            }

            var columnCount = rows.First().Length;

            return rows.Select(
                row => GetRowOrThrowIfBadLength(row, columnCount));
        }

        private static string[] GetRowOrThrowIfBadLength(string[] cells, int expectedLength)
        {
            return cells.Length == expectedLength 
                ? cells
                : throw new StructureException($"All rows in a CSV file must have an equal number of cells. Expected number of cells (based on the first row): {expectedLength}");
        }
    }
}