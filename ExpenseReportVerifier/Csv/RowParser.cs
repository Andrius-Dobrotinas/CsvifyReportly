using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Csv
{
    public interface IRowParser
    {
        string[] Parse(string row, char delimiter);
    }


    public class RowParser : IRowParser
    {
        private const string quotationMark = "\"";

        public string[] Parse(string row, char delimiter)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            if (row.Length == 0) return new string[0];

            var segments = new List<string>();

            int currentPosition = 0;
            int openingQuotationMark = row.IndexOf(quotationMark, currentPosition);

            while (true)
            {
                int delimiterIndex = row.IndexOf(delimiter, currentPosition);
                
                if (delimiterIndex == -1) // this is the last segment
                {
                    string segment = row.Substring(currentPosition);
                    segments.Add(segment);
                    break;
                }

                // a closing quotation mark is presumed here
                bool isDelimiterBetweenQuatationMarks = openingQuotationMark > -1 && delimiterIndex > openingQuotationMark;

                if (isDelimiterBetweenQuatationMarks)
                {
                    currentPosition++; // advance current position past the opening quotation mark

                    int closingQuotationMarkIndex = row.IndexOf(quotationMark, openingQuotationMark + 1);
                    int currentLength = closingQuotationMarkIndex + 1;
                    delimiterIndex = closingQuotationMarkIndex + 1;

                    if (row.Length > currentLength
                        && row.ElementAt(delimiterIndex) != delimiter)
                        throw new UnexpectedTokenException($"Expected to find a delimiter after the closing quotation mark at position {delimiterIndex}");

                    string segment = row.Substring(currentPosition, delimiterIndex - currentPosition - 1); // -1 for the delimiter char
                    segments.Add(segment);

                    currentPosition = delimiterIndex + 1; // advance current position right past the delimiter

                    if (row.Length > currentPosition - 1)
                        openingQuotationMark = row.IndexOf(quotationMark, currentPosition); // Get the next opening quotation mark
                    else
                        break;
                }
                else
                {
                    string segment = row.Substring(currentPosition, delimiterIndex - currentPosition);
                    segments.Add(segment);

                    currentPosition = delimiterIndex + 1; // advance current position right past the delimiter
                }
            }

            return segments.ToArray();
        }
    }
}