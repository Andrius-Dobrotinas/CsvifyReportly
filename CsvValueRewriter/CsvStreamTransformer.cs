﻿using System;
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
            CsvDocument document = CsvStreamParser.ReadCsvDocument(source, delimiter);
            
            foreach (var rewriter in transformers)
                document = rewriter.TransformRows(document);

            return WriteToCsvStream(document, delimiter);
        }

        // todo: move this to a separate component
        private Stream WriteToCsvStream(CsvDocument document, char delimiter)
        {
            // todo: unit-test this
            var rows = CombineColumnAndDataRows(document);

            string[] lines = rows.Select(row => stringyfier.Stringifififiify(row, delimiter))
                .ToArray();

            return IO.CsvFileWriter.Write(lines);
        }

        private string[][] CombineColumnAndDataRows(CsvDocument document)
        {
            var allRows = new string[document.Rows.Length + 1][];

            allRows[0] = document.ColumnNames;

            document.Rows.CopyTo(allRows, 1);            

            return allRows;
        }
    }
}