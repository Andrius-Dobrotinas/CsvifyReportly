using Andy.Csv.Transformation.Row.Document.Cmd.Transformer;
using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd.Configuration
{
    public class Settings
    {
        public char CsvDelimiter { get; set; }
        public IDictionary<string, TransformerSettings[]> Profiles { get; set; }
    }
}