using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Serialization
{
    public class RowParser : IRowParser
    {
        private readonly char delimiter;

        public RowParser(char delimiter)
        {
            this.delimiter = delimiter;
        }

        public string[] Parse(string row)
        {
            return Parse(row, delimiter);
        }

        private const char quotationMark = '"';
        private const int negativeIndex = -1;

        /// <summary>
        /// Escaped quotation marks are not supported at this point
        /// </summary>
        public static string[] Parse(string row, char delimiter)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (row.Length == 0) return new string[0];

            var segments = new List<string>();

            int lastCharPosish = row.Length - 1;
            int currentPosish = 0;

            while (currentPosish <= lastCharPosish)
            {
                int delimiterIndex;

                if (row[currentPosish] == quotationMark)
                {
                    int closingQuotationMark = row.IndexOf(quotationMark, currentPosish + 1);
                    // todo: should check whether the quotation mark is escaped

                    if (closingQuotationMark == negativeIndex)
                        throw new MissingTokenException($"Encountered an opening quotation mark at position {currentPosish}, therefore expected to see a closing one at some point, but found none");

                    bool isLastChar = closingQuotationMark == lastCharPosish;
                    if (isLastChar == true)
                    {
                        delimiterIndex = negativeIndex;
                    }
                    else
                    {
                        int nextCharPosish = closingQuotationMark + 1;

                        char nextChar = row[nextCharPosish];

                        if (nextChar != delimiter)
                            throw new UnexpectedTokenException($"Expected to find a delimiter at position {nextCharPosish} right after a closing quotation mark, but found {nextChar}");

                        delimiterIndex = nextCharPosish;
                    }
                }
                else
                {
                    delimiterIndex = row.IndexOf(delimiter, currentPosish);
                }

                int currentLastCharPosish = (delimiterIndex == negativeIndex)
                        ? lastCharPosish
                        : delimiterIndex - 1;

                string segment = row.Substring(currentPosish, currentLastCharPosish - currentPosish + 1);
                segments.Add(segment);

                // advance current position: +1 > delimiter, +1 > past current delimiter
                currentPosish = currentLastCharPosish + 2;
            }

            // if the row ends with a delimiter, that means there's one more blank entry
            if (row.EndsWith(delimiter))
                segments.Add("");

            return segments
                .Select(StripWrappingQuotationMarks)
                .ToArray();
        }

        private static string StripWrappingQuotationMarks(string value)
        {
            return value.Trim('"');
        }
    }
}