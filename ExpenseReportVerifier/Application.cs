﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Cmd
{
    public static class Application
    {
        public static void Go(
            FileInfo statementFile,
            FileInfo transactionsFile,
            FileInfo reportFile,
            ApplicationSettings settings)
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
                result = Comparer.Compare(
                    sourceData.Transactions,
                    sourceData.StatementEntries, 
                    settings.TransactionsCsvFile.ColumnIndexes, 
                    settings.StatementCsvFile.ColumnIndexes);
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