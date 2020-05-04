using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class ReportingComparer<TItem1, TItem2> : IReportingComparer
    {
        private readonly IComparer<TItem1, TItem2> comparer;

        public ReportingComparer(
            IComparer<TItem1, TItem2> comparer)
        {
            this.comparer = comparer;
        }

        public Stream Compare(
            Stream source1,
            Stream source2,
            char source1ValueDelimiter,
            char source2ValueDelimiter,
            char reportValueDelimiter)
        {
            IList<string[]> transactions1;
            int transactions1ColumnCount;

            IList<string[]> transactions2;
            int transactions2ColumnCount;

            try
            {
                transactions1 = CsvStreamReader.Read(
                    source1,
                    source1ValueDelimiter,
                    out transactions1ColumnCount);

                transactions2 = CsvStreamReader.Read(
                    source2,
                    source2ValueDelimiter,
                    out transactions2ColumnCount);
            }
            catch (Andy.Csv.IO.RowReadingException e)
            {
                throw new SourceDataReadException(e.Message, e.InnerException);
            }
            catch (Exception e)
            {
                throw new SourceDataReadException(e);
            }

            ComparisonResult result;
            try
            {
                result = comparer.Compare(
                    transactions1,
                    transactions2);
            }
            catch (Exception e)
            {
                throw new DataComparisonException(e);
            }

            var stringyfyer = new Andy.Csv.RowStringifier(
                new Andy.Csv.ValueEncoder());            

            try
            {
                string[] lines = ResultStringification.StringyfyyResults(
                    result,
                    transactions1ColumnCount,
                    transactions2ColumnCount,
                    reportValueDelimiter,
                    stringyfyer);

                return Andy.Csv.IO.CsvFileWriter.Write(lines);
            }
            catch (Exception e)
            {
                throw new ReportProductionException(e);
            }
        }
    }
}