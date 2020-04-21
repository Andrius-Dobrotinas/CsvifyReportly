using System;

namespace Andy.ExpenseReport.Cmd
{
    public class ReportFileProductionException : MyApplicationException
    {
        public ReportFileProductionException(Exception e)
            : base("An error occured while producing a report file", e)
        {
        }
    }
}