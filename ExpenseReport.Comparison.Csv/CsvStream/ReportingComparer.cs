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

        public Stream Compare<TColumnIndexMap1, TColumnIndexMap2>(
            Stream statement,
            Stream transactions,
            Parameters<TColumnIndexMap1, TColumnIndexMap2> settings)
        {
            SourceData sourceData;
            try
            {
                sourceData = SourceDataReader.ReadSourceData(
                    statement,
                    transactions,
                    settings.StatementCsvFile,
                    settings.TransactionsCsvFile);
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
                    settings.OutputCsvDelimiter,
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