using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public class InputParsingException : Exception
    {
        public InputParsingException(int lineNumber, int sourceNumber, Exception e)
            : base($"Error parsing source line {lineNumber} in source {sourceNumber}", e)
        {

        }
    }
}
