using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.Csv.Rewrite.Value
{
    public interface IValueRewriter
    {
        string Rewrite(string value);
    }

    public class ValueRewriter : IValueRewriter
    {
        private readonly string sourceFormat;
        private readonly string targetFormat;

        public ValueRewriter(string sourceFormat, string targetFormat)
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