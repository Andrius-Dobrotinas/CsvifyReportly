using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public class AmountInverter : IValueTransformer
    {
        private readonly IFormatProvider formatProvider;

        public AmountInverter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        public string GetValue(string value)
        {
            var figure = decimal.Parse(value, formatProvider);

            return (figure * -1).ToString();
        }
    }
}