using System;

namespace Andy.Csv.Rewrite
{
    public class Settings
    {
        public string SourceFormat { get; set; }
        public string TargetFormat { get; set; }
        public char CsvDelimiter { get; set; }
        public int TargetColumnIndex { get; set; }
    }
}