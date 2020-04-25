using System;

namespace Andy.ExpenseReport.Comparison.Csv.File
{
    public class ReportFileProductionException : MyApplicationException
    {
        public ReportFileProductionException(Exception e)
            : base("An error occured while producing a report file", e)
        {
        }
    }
}