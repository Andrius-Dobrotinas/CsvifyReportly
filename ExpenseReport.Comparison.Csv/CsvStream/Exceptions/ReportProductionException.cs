using System;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class ReportProductionException : CsvStreamComparisonException
    {
        public ReportProductionException(Exception e)
            : base("An error occured while producing a report", e)
        {
        }
    }
}