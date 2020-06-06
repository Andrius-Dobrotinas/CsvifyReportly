using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.Rewrite
{
    public class CsvStreamRewriter
    {
        private readonly IRowStringifier stringyfier;
        private readonly IEnumerable<ICsvRewriter> rewriters;

        public CsvStreamRewriter(IRowStringifier stringyfier, IEnumerable<ICsvRewriter> rewriters)
        {
            this.stringyfier = stringyfier;
            this.rewriters = rewriters;
        }

        public Stream Go(Stream source, char delimiter)
        {
            IEnumerable<string[]> rows = CsvStreamParser.ReadRowsFromStream(source, delimiter);

            foreach (var rewriter in rewriters)
                rows = rewriter.Rewrite(rows);

            return WriteToCsvStream(rows, delimiter);
        }

        private Stream WriteToCsvStream(IEnumerable<string[]> rows, char delimiter)
        {
            string[] lines = rows.Select(row => stringyfier.Stringifififiify(row, delimiter))
                .ToArray();

            return IO.CsvFileWriter.Write(lines);
        }
    }
}