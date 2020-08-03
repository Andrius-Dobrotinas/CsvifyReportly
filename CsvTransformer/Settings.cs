using Andy.Csv.Transformation.Row.Document.Setup;
using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public class Settings
    {
        public char CsvDelimiter { get; set; }
        public IDictionary<string, TransformerSettings[]> Profiles { get; set; }
    }
}