using System;
using System.IO;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class Parameters
    {
        public Command Command { get; set; }
        public FileInfo Source2File { get; set; }
        public FileInfo Source1File { get; set; }
        public FileInfo ComparisonReportFile { get; set; }
    }
}