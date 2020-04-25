using System;

namespace Andy.ExpenseReport.Comparison.Csv.File
{
    public class DataProcessingException : MyApplicationException
    {
        public DataProcessingException(Exception e)
            : base("Failed to perform the comparison of data", e)
        {
        }
    }
}