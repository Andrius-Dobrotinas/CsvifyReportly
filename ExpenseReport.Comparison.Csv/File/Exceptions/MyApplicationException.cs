using System;

namespace Andy.ExpenseReport.Comparison.Csv.File
{
    public abstract class MyApplicationException : Exception
    {
        public string Details => InnerException?.Message;

        protected MyApplicationException(string message, Exception e) : base(message, e)
        {

        }
    }
}