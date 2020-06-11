using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation
{
    public class DateTransformer : IValueTransformer
    {
        private readonly string sourceFormat;
        private readonly string targetFormat;

        public DateTransformer(string sourceFormat, string targetFormat)
        {
            this.sourceFormat = sourceFormat;
            this.targetFormat = targetFormat;
        }

        public string GetValue(string value)
        {
            DateTimeOffset date;
            if (DateTimeOffset.TryParseExact(value, sourceFormat, null, System.Globalization.DateTimeStyles.None, out date))
                return date.ToString(targetFormat);
            else
                return value; // todo: error reporting?
        }
    }
}