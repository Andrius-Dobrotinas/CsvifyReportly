using System;

namespace Andy.ExpenseReport.Cmd
{
    public class DataProcessingException : MyApplicationException
    {
        public DataProcessingException(Exception e)
            : base("Failed to perform the comparison of data", e)
        {
        }
    }
}