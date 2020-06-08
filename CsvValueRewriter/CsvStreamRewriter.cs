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
        private readonly IEnumerable<Filter.IRowMatchEvaluator> rowMatchers;

        public CsvStreamRewriter(
            IRowStringifier stringyfier,
            IEnumerable<ICsvRewriter> rewriters,
            IEnumerable<Filter.IRowMatchEvaluator> rowMatchers)
        {
            this.stringyfier = stringyfier;
            this.rewriters = rewriters;
            this.rowMatchers = rowMatchers;
        }

        public Stream Go(Stream source, char delimiter)
        {
            IEnumerable<string[]> rows = CsvStreamParser.ReadRowsFromStream(source, delimiter);

            foreach (var matcher in rowMatchers)
                rows = rows.Where(matcher.IsMatch);

            foreach (var rewriter in rewriters)
                rows = rewriter.Rewrite(rows);

            return WriteToCsvStream(rows, delimiter);
        }

        // todo: move this to a separate component
        private Stream WriteToCsvStream(IEnumerable<string[]> rows, char delimiter)
        {
            string[] lines = rows.Select(row => stringyfier.Stringifififiify(row, delimiter))
                .ToArray();

            return IO.CsvFileWriter.Write(lines);
        }
    }
}