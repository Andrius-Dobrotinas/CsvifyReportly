using System;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public abstract class CsvStreamComparisonException : Exception
    {
        public Exception ActualException => InnerException;

        protected CsvStreamComparisonException(string message, Exception e) : base(message, e)
        {

        }
    }
}