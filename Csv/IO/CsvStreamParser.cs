using System;
using System.IO;

namespace Andy.Csv.IO
{
    public static class CsvStreamParser
    {
        public static string[][] ReadNParse(Stream source, char delimiter)
        {
            return CsvStreamReader.Read(
                source,
                line => RowParser.Parse(line, delimiter));
        }
    }
}