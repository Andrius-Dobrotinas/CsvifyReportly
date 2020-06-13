using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public class Settings
    {
        public char CsvDelimiter { get; set; }
        public TransformationSettings Transformers { get; set; }
        public IDictionary<string, string[]> Profiles { get; set; }

        public class TransformationSettings
        {
            public DateRewriterSettings DateRewriter { get; set; }
            public CurrencyAmountThingSettings TheCurrencyAmountThing { get; set; }
            public ColumnReducerSettings ColumnReducer { get; set; }
            public ColumnInserterSettings ColumnInserter { get; set; }
            public InvertedSingleRowValueFilterSettings InvertedSingleValueFilter { get; set; }

            public class InvertedSingleRowValueFilterSettings
            {
                public int TargetColumnIndex { get; set; }
                public string TargetValue { get; set; }
            }

            public class DateRewriterSettings
            {
                public string TargetColumnName { get; set; }
                public string SourceFormat { get; set; }
                public string TargetFormat { get; set; }
            }

            public class CurrencyAmountThingSettings
            {
                public string AmountColumnName { get; set; }
                public string CurrencyColumnName { get; set; }
                public string ResultAmountColumnName { get; set; }
                public string TargetCurrency { get; set; }
            }

            public class ColumnInserterSettings
            {
                public int TargetColumnIndex { get; set; }
            }

            public class ColumnReducerSettings
            {
                public int[] TargetColumnIndexes { get; set; }
            }
        }
    }
}