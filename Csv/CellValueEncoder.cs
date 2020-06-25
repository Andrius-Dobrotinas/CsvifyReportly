using System;
using System.Collections.Generic;

namespace Andy.Csv
{
    public interface ICellValueEncoder
    {
        string Encode(string value, char delimiter);
    }

    public class CellValueEncoder : ICellValueEncoder
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