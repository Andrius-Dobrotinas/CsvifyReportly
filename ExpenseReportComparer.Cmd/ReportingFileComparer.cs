using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class ReportingFileComparer
    {
        private readonly IReportingComparer comparer;

        public ReportingFileComparer(IReportingComparer comparer)
        {
            this.comparer = comparer;
        }

        public void CompareAndWriteReport(
            FileInfo source1,
            FileInfo source2,
            FileInfo reportFile,
            char source1ValueDelimiter,
            char source2ValueDelimiter,
            char reportValueDelimiter)
        {
            using (var statementStream = source1.OpenRead())
            using (var transactionStream = source2.OpenRead())
            using (var reportStream = comparer.Compare(
                statementStream,
                transactionStream,
                source1ValueDelimiter,
                source2ValueDelimiter,
                reportValueDelimiter))
            using (var outputStream = reportFile.OpenWrite())
                reportStream.CopyTo(outputStream);
        }
    }
}