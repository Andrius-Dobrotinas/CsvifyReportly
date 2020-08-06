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
        public void ReportStart(string transformerName)
        {
            Console.WriteLine(@$"Running transformer ""{transformerName}""");
        }

        public void ReportFinish(CsvDocument before, CsvDocument after)
        {
            Console.WriteLine("Transformer finished");

            ReportDifferences(before, after);
        }

        protected abstract void ReportDifferences(CsvDocument before, CsvDocument after);

        protected string Stringicize(string[] row)
        {
            return string.Join('|', row);
        }
    }
}