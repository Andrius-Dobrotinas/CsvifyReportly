using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    public interface IResultReporter
    {
        void ReportStart(string transformerName);
        void ReportFinish(CsvDocument before, CsvDocument after);
    }

    public abstract class ResultReporter : IResultReporter
    {
        protected readonly IStringWriter stringWriter;

        public ResultReporter(IStringWriter stringWriter)
        {
            this.stringWriter = stringWriter ?? throw new ArgumentNullException(nameof(stringWriter));
        }

        public void ReportStart(string transformerName)
        {
            stringWriter.WriteLine(@$"Running transformer ""{transformerName}""");
        }

        public void ReportFinish(CsvDocument before, CsvDocument after)
        {
            ReportDifferences(before, after);

            stringWriter.WriteLine("------------------");
        }

        protected abstract void ReportDifferences(CsvDocument before, CsvDocument after);

        protected string Stringicize(string[] row)
        {
            return string.Join(" | ", row);
        }
    }
}