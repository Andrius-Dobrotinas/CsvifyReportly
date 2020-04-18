using System;
using System.IO;

namespace Andy.ExpenseReport
{
    public class Parameters
    {
        public FileInfo TransactionFile { get; set; }
        public FileInfo StatementFile { get; set; }
        public FileInfo ReportFile { get; set; }
        public char InputCsvDelimiter { get; set; }
        public char OutputCsvDelimiter { get; set; }
    }
}