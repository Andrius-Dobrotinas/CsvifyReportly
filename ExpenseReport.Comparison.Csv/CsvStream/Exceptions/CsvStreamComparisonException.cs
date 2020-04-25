using System;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public abstract class CsvStreamComparisonException : Exception
    {
        public string Details => InnerException?.Message;

        protected CsvStreamComparisonException(string message, Exception e) : base(message, e)
        {

        }
    }
}