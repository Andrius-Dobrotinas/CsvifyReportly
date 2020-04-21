using System;

namespace Andy.ExpenseReport.Cmd
{
    public class ReportFileWriteException : MyApplicationException
    {
        public ReportFileWriteException(Exception e)
            : base("Failed to write the report file", e)
        {
        }
    }
}