using System;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public interface IComparisonItemWithSourceData
    {
        string[] SourceData { get; }
    }
}