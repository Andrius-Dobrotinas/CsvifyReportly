using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public class AmountInverter : IValueTransformer
    {
        public string GetValue(string value)
        {
            var figure = decimal.Parse(value);

            return (figure * -1).ToString();
        }
    }
}