using System;

namespace Andy.ExpenseReport.Csv
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException(string msg) : base(msg)
        {

        }
    }
}