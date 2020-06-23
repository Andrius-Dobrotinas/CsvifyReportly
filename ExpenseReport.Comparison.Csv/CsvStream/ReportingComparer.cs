using Andy.Csv.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class ReportingComparer<TItem1, TItem2> : IReportingComparer
    {
        private readonly IComparer<TItem1, TItem2> comparer;
        private readonly ISafeCsvStreamReader csvStream1Reader;
        private readonly ISafeCsvStreamReader csvStream2Reader;

        public ReportingComparer(
            IComparer<TItem1, TItem2> comparer,
            ISafeCsvStreamReader csvStream1Reader,
            ISafeCsvStreamReader csvStream2Reader)
        {
            this.comparer = comparer;
            this.csvStream1Reader = csvStream1Reader;
            this.csvStream2Reader = csvStream2Reader;
        }

        public Stream Compare(
            Stream source1,
            Stream source2,
            char reportValueDelimiter)
        {
            IList<string[]> transactions1;

            IList<string[]> transactions2;

            transactions1 = Read(
                    csvStream1Reader,
                    1,
                    source1);

            transactions2 = Read(
                    csvStream2Reader,
                    2,
                    source2);

            ComparisonResult result;
            try
            {
                result = comparer.Compare(
                    transactions1,
                    transactions2);
            }
            catch (InputParsingException)
            {
                throw;
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
                    transactions1.Count,
                    transactions2.Count,
                    reportValueDelimiter,
                    stringyfyer);

                return Andy.Csv.IO.CsvFileWriter.Write(lines);
            }
            catch (Exception e)
            {
                throw new ReportProductionException(e);
            }
        }

        private static IList<string[]> Read(
            ISafeCsvStreamReader csvStreamReader,
            int sourceNumber,
            Stream source)
        {
            try
            {
                return csvStreamReader.Read(source);
            }
            catch (Andy.Csv.IO.RowReadingException e)
            {
                throw new SourceDataReadException(e.Message, sourceNumber, e.InnerException);
            }
            catch (Exception e)
            {
                throw new SourceDataReadException(sourceNumber, e);
            }
        }
    }
}