using System;
using System.IO;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class Parameters
    {
        public FileInfo TransactionFile { get; set; }
        public FileInfo StatementFile { get; set; }
        public FileInfo ComparisonReportFile { get; set; }
    }
}