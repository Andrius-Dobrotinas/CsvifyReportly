﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public class CsvStreamTransformer
    {
        private readonly Serialization.IRowStringifier stringyfier;
        private readonly IEnumerable<IDocumentTransformer> transformers;
        private readonly IO.ICsvDocumentByteStreamReader csvReader;

        public CsvStreamTransformer(
            Serialization.IRowStringifier stringyfier,
            IEnumerable<IDocumentTransformer> transformers,
            IO.ICsvDocumentByteStreamReader csvReader)
        {
            this.stringyfier = stringyfier;
            this.transformers = transformers;
            this.csvReader = csvReader;
        }

        public Stream Go(Stream source, char delimiter)
        {
            CsvDocument document = csvReader.Read(source);

            foreach (var rewriter in transformers)
                document = rewriter.Transform(document);

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
            var allRows = new string[document.ContentRows.Length + 1][];

            allRows[0] = document.HeaderCells;

            document.ContentRows.CopyTo(allRows, 1);

            return allRows;
        }
    }
}