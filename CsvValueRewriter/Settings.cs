using System;

namespace Andy.Csv.Rewrite
{
    public class Settings
    {
        
        public char CsvDelimiter { get; set; }
        public RewriterSettings Rewriters { get; set; }
        public string[] RewriterChain { get; set; }

        public class RewriterSettings
        {
            public DateRewriterSettings DateRewriter { get; set; }
            public CurrencyAmountThingSettings TheCurrencyAmountThing { get; set; }

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
        }
    }
}