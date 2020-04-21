using System;

namespace Andy.ExpenseReport.Verifier
{
    public class SourceDataReadException : MyApplicationException
    {
        public SourceDataReadException(Exception e)
            : base("Failed to read the source data", e)
        {
        }
    }
}