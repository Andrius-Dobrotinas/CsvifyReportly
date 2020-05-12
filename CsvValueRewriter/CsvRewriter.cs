using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.Rewrite.Value
{
    public class CsvRewriter
    {
        private readonly IRowStringifier stringyfier;
        private readonly ICsvRowRewriter csvRowRewriter;

        public CsvRewriter(IRowStringifier stringyfier, ICsvRowRewriter csvRowRewriter)
        {
            this.stringyfier = stringyfier;
            this.csvRowRewriter = csvRowRewriter;
        }

        public Stream Go(Stream source, char delimiter)
        {
            IEnumerable<string[]> rows = CsvStreamParser.ReadRowsFromStream(source, delimiter);

            IEnumerable<string[]> result = csvRowRewriter.Rewrite(rows);

            string[] lines = result.Select(row => stringyfier.Stringifififiify(row, delimiter))
                .ToArray();

            return IO.CsvFileWriter.Write(lines);
        }
    }
}