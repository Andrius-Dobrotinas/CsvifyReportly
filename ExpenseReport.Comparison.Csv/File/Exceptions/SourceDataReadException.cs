using System;

namespace Andy.ExpenseReport.Comparison.Csv.File
{
    public class SourceDataReadException : MyApplicationException
    {
        public SourceDataReadException(Exception e)
            : base("Failed to read the source data", e)
        {
        }
    }
}