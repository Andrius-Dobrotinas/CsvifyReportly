using System;
using System.Collections.Generic;

namespace Andy.Csv.IO
{
    public class RowReadingException : Exception
    {
        public RowReadingException(int rowNumber, Exception e)
            : base($"Error reading row number {rowNumber}", e)
        {

        }
    }
}
