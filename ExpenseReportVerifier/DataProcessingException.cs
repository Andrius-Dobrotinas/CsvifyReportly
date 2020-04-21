﻿namespace Andy.ExpenseReport.Cmd
{
    public class DataProcessingException : ConsoleApplicationLevelException
    {
        public DataProcessingException(string exceptionDetails)
            : base(-200, "Failed to perform the comparison of data", exceptionDetails)
        {
        }
    }
}