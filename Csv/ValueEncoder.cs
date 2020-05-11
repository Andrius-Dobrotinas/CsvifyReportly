using System;
using System.Collections.Generic;

namespace Andy.Csv
{
    public interface IValueEncoder
    {
        string Encode(string value, char delimiter);
    }

    public class ValueEncoder : IValueEncoder
    {
        public string Encode(string value, char delimiter)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value.Contains('"'))
                throw new NotImplementedException("Escaping of quotation marks");

            if (value.Contains(delimiter))
                value = $"\"{value}\"";
            
            return value;
        }
    }
}