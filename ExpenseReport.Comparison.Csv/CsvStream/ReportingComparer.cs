using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class ReportingComparer<TItem1, TItem2> : IReportingComparer<TItem1, TItem2>
    {
        private readonly IComparer<TItem1, TItem2> comparer;

        public ReportingComparer(IComparer<TItem1, TItem2> comparer)
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
            SourceData sourceData;
            try
            {
                sourceData = SourceDataReader.ReadSourceData(
                    source1,
                    source2,
                    source1ValueDelimiter,
                    source2ValueDelimiter);
            }
            catch (Exception e)
            {
                throw new SourceDataReadException(e);
            }

            ComparisonResult result;
            try
            {
                result = comparer.Compare(
                    sourceData.Transactions,
                    sourceData.StatementEntries);
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
                    sourceData.StatementColumnCount,
                    sourceData.TransactionColumnCount,
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