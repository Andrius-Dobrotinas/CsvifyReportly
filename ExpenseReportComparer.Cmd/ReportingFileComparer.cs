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
            char reportValueDelimiter)
        {
            using (var source1Stream = source1.OpenRead())
            using (var source2Stream = source2.OpenRead())
            using (var reportStream = comparer.Compare(
                source1Stream,
                source2Stream,
                reportValueDelimiter))
            using (var outputStream = reportFile.OpenWrite())
                reportStream.CopyTo(outputStream);
        }
    }
}