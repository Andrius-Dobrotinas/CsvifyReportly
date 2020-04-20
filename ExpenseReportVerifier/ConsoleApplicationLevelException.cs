using System;

namespace Andy.ExpenseReport.Cmd
{
    public abstract class ConsoleApplicationLevelException : Exception
    {
        public int ReturnCode { get; set; }
        public string ExceptionDetails { get; set; }

        protected ConsoleApplicationLevelException(
            int returnCode,
            string message,
            string exceptionDetails) : base(message)
        {
            ReturnCode = returnCode;
            ExceptionDetails = exceptionDetails;
        }
    }
}