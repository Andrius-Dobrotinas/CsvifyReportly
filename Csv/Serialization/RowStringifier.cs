using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Serialization
{
    public interface IRowStringifier
    {
        string Stringifififiify(IEnumerable<string> row, char delimiter);
    }

    public class RowStringifier : IRowStringifier
    {
        private readonly ICellValueEncoder encoder;

        public RowStringifier(ICellValueEncoder encoder)
        {
            this.encoder = encoder;
        }

        public string Stringifififiify(IEnumerable<string> row, char delimiter)
        {
            var normalized = row.Select(
                value => string.IsNullOrEmpty(value)
                    ? value
                    : encoder.Encode(value, delimiter));

            return string.Join(delimiter, normalized);
        }
    }
}