using System;
using System.Collections.Generic;

namespace Andy.Csv.Rewrite
{
    public class Settings
    {
        
        public char CsvDelimiter { get; set; }
        public RewriterSettings Rewriters { get; set; }
        public IDictionary<string, string[]> RewriterChains { get; set; }
        public FilterSettings Filters { get; set; }
        public IDictionary<string, string[]> FilterChains { get; set; }

        public class RewriterSettings
        {
            public DateRewriterSettings DateRewriter { get; set; }
            public CurrencyAmountThingSettings TheCurrencyAmountThing { get; set; }
            public ColumnReducerSettings ColumnReducer { get; set; }
            public ColumnInserterSettings ColumnInserter { get; set; }

            public class DateRewriterSettings
            {
                public int TargetColumnIndex { get; set; }
                public string SourceFormat { get; set; }
                public string TargetFormat { get; set; }
            }

            public class CurrencyAmountThingSettings
            {
                public int AmountColumnIndex { get; set; }
                public int CurrencyColumnIndex { get; set; }
                public int ResultAmountColumnIndex { get; set; }
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

        public class FilterSettings
        {
            public InvertedSingleRowValueEvaluatorSettings InvertedSingleValue { get; set; }

            public class InvertedSingleRowValueEvaluatorSettings
            {
                public int TargetColumnIndex { get; set; }
                public string TargetValue { get; set; }
            }
        }
    }
}