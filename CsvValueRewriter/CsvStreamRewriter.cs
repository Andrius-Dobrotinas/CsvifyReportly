using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.Rewrite.Value
{
    public class CsvStreamRewriter
    {
        private readonly IRowStringifier stringyfier;
        private readonly ICsvRewriter rewriter;

        public CsvStreamRewriter(IRowStringifier stringyfier, ICsvRewriter rewriter)
        {
            this.stringyfier = stringyfier;
            this.rewriter = rewriter;
        }

        public Stream Go(Stream source, char delimiter)
        {
            IEnumerable<string[]> rows = CsvStreamParser.ReadRowsFromStream(source, delimiter);

            IEnumerable<string[]> result = rewriter.Rewrite(rows);

            string[] lines = result.Select(row => stringyfier.Stringifififiify(row, delimiter))
                .ToArray();

            return IO.CsvFileWriter.Write(lines);
        }
    }
}