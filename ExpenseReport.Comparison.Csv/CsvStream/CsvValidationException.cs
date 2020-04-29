using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class CsvValidationException : Exception
    {
        public CsvValidationException(string msg) : base(msg)
        {

        }
    }
}