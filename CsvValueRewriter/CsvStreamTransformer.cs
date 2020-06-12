using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public class CsvStreamTransformer
    {
        private readonly IRowStringifier stringyfier;
        private readonly IEnumerable<IDocumentTransformer> transformers;

        public CsvStreamTransformer(
            IRowStringifier stringyfier,
            IEnumerable<IDocumentTransformer> transformers)
        {
            this.stringyfier = stringyfier;
            this.transformers = transformers;
        }

        public Stream Go(Stream source, char delimiter)
        {
            IEnumerable<string[]> rows = CsvStreamParser.ReadRowsFromStream(source, delimiter);
            
            foreach (var rewriter in transformers)
                rows = rewriter.TransformRows(rows);

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