using System;
using System.IO;

namespace Andy.ExpenseReport.Cmd
{
    public class Parameters
    {
        public FileInfo TransactionFile { get; set; }
        public FileInfo StatementFile { get; set; }
        public FileInfo ComparisonReportFile { get; set; }
    }
}