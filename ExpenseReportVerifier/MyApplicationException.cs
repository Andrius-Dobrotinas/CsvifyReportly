using System;

namespace Andy.ExpenseReport.Verifier
{
    public abstract class MyApplicationException : Exception
    {
        public string Details => InnerException?.Message;

        protected MyApplicationException(string message, Exception e) : base(message, e)
        {

        }
    }
}