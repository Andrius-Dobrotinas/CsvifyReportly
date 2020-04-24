using System;

namespace Andy.ExpenseReport.Verifier.Statement
{
    public interface IComparisonItemWithSourceData
    {
        string[] SourceData { get; }
    }
}