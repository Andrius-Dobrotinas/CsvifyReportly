using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class ReportingComparer<TItem1, TItem2> : IReportingComparer
    {
        private readonly IComparer<TItem1, TItem2> comparer;
        private readonly ICsvStreamReader csvStream1Reader;
        private readonly ICsvStreamReader csvStream2Reader;

        public ReportingComparer(
            IComparer<TItem1, TItem2> comparer,
            ICsvStreamReader csvStream1Reader,
            ICsvStreamReader csvStream2Reader)
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
            int transactions1ColumnCount;

            IList<string[]> transactions2;
            int transactions2ColumnCount;

            transactions1 = Read(
                    csvStream1Reader,
                    1,
                    source1,
                    out transactions1ColumnCount);

            transactions2 = Read(
                    csvStream2Reader,
                    2,
                    source2,
                    out transactions2ColumnCount);

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

        private static IList<string[]> Read(
            ICsvStreamReader csvStreamReader,
            int sourceNumber,
            Stream source,            
            out int columnCount)
        {
            try
            {
                return csvStreamReader.Read(
                    source,
                    out columnCount);
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