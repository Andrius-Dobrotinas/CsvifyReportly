using System;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class SourceDataReadException : CsvStreamComparisonException
    {
        public SourceDataReadException(Exception e)
            : base("An error occured while reading the source data", e)
        {
        }
    }
}