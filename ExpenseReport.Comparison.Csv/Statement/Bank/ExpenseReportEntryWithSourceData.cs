using Andy.ExpenseReport.Comparison.Statement.Bank;
using System;

namespace Andy.ExpenseReport.Comparison.Csv.Statement.Bank
{
    public class ExpenseReportEntryWithSourceData :
        ExpenseReportEntry,
        IHaveSourceData,
        IHaveDate
    {
        public string[] SourceData { get; set; }
    }
}