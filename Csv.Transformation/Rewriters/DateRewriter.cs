using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Rewriters
{
    public class DateRewriter : IValueRewriter
    {
        private readonly string sourceFormat;
        private readonly string targetFormat;

        public DateRewriter(string sourceFormat, string targetFormat)
        {
            this.sourceFormat = sourceFormat;
            this.targetFormat = targetFormat;
        }

        public string Rewrite(string value)
        {
            DateTimeOffset date;
            if (DateTimeOffset.TryParseExact(value, sourceFormat, null, System.Globalization.DateTimeStyles.None, out date))
                return date.ToString(targetFormat);
            else
                return value; // todo: error reporting?
        }
    }
}