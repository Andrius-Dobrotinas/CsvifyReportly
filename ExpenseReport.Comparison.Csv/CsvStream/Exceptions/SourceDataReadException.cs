using System;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class SourceDataReadException : CsvStreamComparisonException
    {
        public SourceDataReadException(int sourceNumber, Exception e)
            : base($"An error occured while reading source {sourceNumber}", e)
        {
        }

        public SourceDataReadException(string message, int sourceNumber, Exception e)
            : base($"{message}... in source {sourceNumber}", e)
        {
        }
    }
}