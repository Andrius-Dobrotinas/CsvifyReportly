using System;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public interface IHaveSourceData
    {
        string[] SourceData { get; }
    }
}