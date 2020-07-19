using Andy.Csv;
using Andy.Csv.IO;
using Andy.Csv.Transformation.Row.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class ReportingComparer<TItem1, TItem2> : IReportingComparer
    {
        private readonly IComparer<TItem1, TItem2> comparer;
        private readonly ICsvDocumentByteStreamReader csvStream1Reader;
        private readonly ICsvDocumentByteStreamReader csvStream2Reader;
        private readonly IMultiTransformer transformer1;

        public ReportingComparer(
            IComparer<TItem1, TItem2> comparer,
            ICsvDocumentByteStreamReader csvStream1Reader,
            ICsvDocumentByteStreamReader csvStream2Reader,
            IMultiTransformer transformer1)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            this.csvStream1Reader = csvStream1Reader ?? throw new ArgumentNullException(nameof(csvStream1Reader));
            this.csvStream2Reader = csvStream2Reader ?? throw new ArgumentNullException(nameof(csvStream2Reader));
            this.transformer1 = transformer1 ?? throw new ArgumentNullException(nameof(transformer1));
        }

        public Stream Compare(
            Stream source1,
            Stream source2,
            char reportValueDelimiter)
        {
            CsvDocument transactions1 = Read(
                    csvStream1Reader,
                    1,
                    source1);

            CsvDocument transactions2 = Read(
                    csvStream2Reader,
                    2,
                    source2);

            var transactionRows1 = transformer1.Transform(transactions1).ContentRows;

            ComparisonResult result;
            try
            {
                result = comparer.Compare(
                    transactionRows1,
                    transactions2.ContentRows);
            }
            catch (InputParsingException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new DataComparisonException(e);
            }

            var stringyfyer = new Andy.Csv.Serialization.RowStringifier(
                new Andy.Csv.Serialization.CellValueEncoder());

            try
            {
                string[] lines = ResultStringification.StringyfyyResults(
                    result,
                    transactions1.ContentRows.Length,
                    transactions2.ContentRows.Length,
                    reportValueDelimiter,
                    stringyfyer);

                return CsvFileWriter.Write(lines);
            }
            catch (Exception e)
            {
                throw new ReportProductionException(e);
            }
        }

        private static CsvDocument Read(
            ICsvDocumentByteStreamReader csvStreamReader,
            int sourceNumber,
            Stream source)
        {
            try
            {
                return csvStreamReader.Read(source);
            }
            catch (RowReadingException e)
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