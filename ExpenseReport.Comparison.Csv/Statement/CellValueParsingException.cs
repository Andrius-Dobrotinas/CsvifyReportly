using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement
{
    public class CellValueParsingException : Exception
    {
        public CellValueParsingException(string value, Type targetType, Exception e)
            : base($"Error parsing value \"{value}\" as {targetType.Name}", e)
        {

        }
    }
}