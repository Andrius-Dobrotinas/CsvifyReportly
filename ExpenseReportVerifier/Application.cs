using Andy.ExpenseReport.Verifier.Statement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Verifier
{
    public class Application<TItem1, TItem2>
    {
        private readonly IComparer<TItem1, TItem2> comparer;

        public Application(IComparer<TItem1, TItem2> comparer)
        {
            this.comparer = comparer;
        }

        public void CompareAndWriteReport<TColumnIndexMap1, TColumnIndexMap2>(
            FileInfo statementFile,
            FileInfo transactionsFile,
            FileInfo reportFile,
            ApplicationParameters1<TColumnIndexMap1, TColumnIndexMap2> settings)
        {
            SourceData sourceData;
            try
            {
                sourceData = SourceDataReader.ReadSourceData(
                    statementFile,
                    transactionsFile,
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
                throw new DataProcessingException(e);
            }

            var stringyfyer = new Csv.RowStringifier(
                new Csv.ValueEncoder());            

            try
            {
                string[] lines = ResultStringification.StringyfyyResults(
                    result,
                    sourceData.StatementColumnCount,
                    sourceData.TransactionColumnCount,
                    settings.OutputCsvDelimiter,
                    stringyfyer);

                Csv.IO.CsvFileWriter.Write(lines, reportFile);
            }
            catch (Exception e)
            {
                throw new ReportFileProductionException(e);
            }
        }
    }
}