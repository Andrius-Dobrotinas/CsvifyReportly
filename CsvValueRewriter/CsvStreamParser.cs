﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.Csv.Rewrite.Value
{
    public static class CsvStreamParser
    {
        public static string[][] ReadRowsFromStream(Stream source, char delimiter)
        {
            return IO.CsvStreamReader.Read(
                source,
                line => Andy.Csv.RowParser.Parse(line, delimiter));
        }
    }
}