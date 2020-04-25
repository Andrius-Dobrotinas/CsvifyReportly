using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Andy.ExpenseReport.Comparison.Csv.File
{
    public class ReportingFileComparer<TItem1, TItem2>
    {
        private readonly IReportingComparer<TItem1, TItem2> comparer;

        public ReportingFileComparer(IReportingComparer<TItem1, TItem2> comparer)
        {
            this.comparer = comparer;
        }

        public void CompareAndWriteReport<TColumnIndexMap1, TColumnIndexMap2>(
            FileInfo statementFile,
            FileInfo transactionsFile,
            FileInfo reportFile,
            Parameters<TColumnIndexMap1, TColumnIndexMap2> settings)
        {
            using (var statementStream = statementFile.OpenRead())
            using (var transactionStream = transactionsFile.OpenRead())
            using (var reportStream = comparer.Compare(
                statementStream,
                transactionStream,
                settings))
            using (var outputStream = reportFile.OpenWrite())
                reportStream.CopyTo(outputStream);
        }
    }
}
