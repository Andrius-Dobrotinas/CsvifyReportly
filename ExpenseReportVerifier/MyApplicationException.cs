using System;

namespace Andy.ExpenseReport.Cmd
{
    public abstract class MyApplicationException : Exception
    {
        public string Details => InnerException?.Message;

        protected MyApplicationException(string message, Exception e) : base(message, e)
        {

        }
    }
}