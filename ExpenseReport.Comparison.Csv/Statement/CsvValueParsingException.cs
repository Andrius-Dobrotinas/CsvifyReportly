using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement
{
    public class CsvValueParsingException : Exception
    {
        public CsvValueParsingException(string value, Type targetType, Exception e)
            : base($"Error parsing value \"{value}\" as {targetType.Name}", e)
        {

        }
    }
}