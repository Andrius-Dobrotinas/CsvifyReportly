using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement
{
    public static class CsvValueParser
    {
        public static DateTime ParseDateOrThrow(string value, string format)
        {
            try
            {
                return DateTime.ParseExact(value, format, null);
            }
            catch (FormatException e)
            {
                throw new CellValueParsingException(value, typeof(DateTime), e);
            }
        }

        public static decimal ParseDecimalOrThrow(string value)
        {
            try
            {
                return decimal.Parse(value.ToLowerInvariant());
            }
            catch (FormatException e)
            {
                throw new CellValueParsingException(value, typeof(decimal), e);
            }
        }

        public static bool ParseBoolOrThrow(string value)
        {
            try
            {
                return bool.Parse(value);
            }
            catch (FormatException e)
            {
                throw new CellValueParsingException(value, typeof(decimal), e);
            }
        }
    }
}