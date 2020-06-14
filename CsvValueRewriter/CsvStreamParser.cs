using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public static class CsvStreamParser
    {
        private static string[][] ReadRowsFromStream(Stream source, char delimiter)
        {
            return IO.CsvStreamReader.Read(
                source,
                line => Andy.Csv.RowParser.Parse(line, delimiter));
        }

        // todo: create a testable component
        public static CsvDocument ReadCsvDocument(Stream source, char delimiter)
        {
            var rows = ReadRowsFromStream(source, delimiter);

            if (rows.Any() == false) return null;

            return new CsvDocument
            {
                HeaderCells = rows.First(),
                ContentRows = rows.Skip(1).ToArray()
            };
        }
    }
}