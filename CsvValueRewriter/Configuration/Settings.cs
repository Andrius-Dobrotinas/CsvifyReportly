using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd.Configuration
{
    public class Settings
    {
        public char CsvDelimiter { get; set; }
        public IDictionary<string, Transformer.TransformerSettings[]> Profiles { get; set; }
    }
    
    namespace Transformer
    {
        public abstract class TransformerSettings
        {
        }

        public class InvertedSingleRowValueFilterSettings : TransformerSettings
        {
            public string TargetColumnName { get; set; }
            public string TargetValue { get; set; }
        }

        public class DateRewriterSettings : TransformerSettings
        {
            public string TargetColumnName { get; set; }
            public string SourceFormat { get; set; }
            public string TargetFormat { get; set; }
        }

        public class CurrencyAmountThingSettings : TransformerSettings
        {
            public string AmountColumnName { get; set; }
            public string CurrencyColumnName { get; set; }
            public string ResultAmountColumnName { get; set; }
            public string TargetCurrency { get; set; }
        }

        public class ColumnInserterSettings : TransformerSettings
        {
            public int TargetColumnIndex { get; set; }
            public string TargetColumnName { get; set; }
        }

        public class ColumnReducerSettings : TransformerSettings
        {
            public string[] TargetColumnNames { get; set; }
        }
    }
}