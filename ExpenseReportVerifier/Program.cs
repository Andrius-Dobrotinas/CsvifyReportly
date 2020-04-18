using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport
{
    class Program
    {
        private const char csvDelimiter = ',';

        static int Main(string[] args)
        {
            var statementFile = new FileInfo(args[1]);
            var transactionsFile = new FileInfo(args[2]);
            var reportFile = new FileInfo(args[3]);

            try
            {
                Act(
                    statementFile,
                    transactionsFile,
                    csvDelimiter,
                    reportFile,
                    csvDelimiter);
            }
            catch (ConsoleApplicationLevelException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.ExceptionDetails);

                return e.ReturnCode;
            }

            Console.WriteLine("Done");

            return 0;
        }

        private static void Act(
            FileInfo statementFile,
            FileInfo transactionsFile,
            char csvDelimiter,
            FileInfo reportFile,
            char reportCsvDelimiter)
        {
            SourceData sourceData;
            try
            {
                sourceData = ReadSourceData(statementFile, transactionsFile, csvDelimiter);
            }
            catch (Exception e)
            {
                throw new SourceDataReadException(e.Message);
            }

            var matcher = new CollectionComparer(
                new MatchFinder(
                    new ItemComparer(
                        new MerchantComparer())));

            ComparisonResult<StatementEntryWithSourceData, TransactionDetailsWithSourceData> result;
            try
            {
                result = matcher.Compare(sourceData.StatementEntries, sourceData.Transactions.ToArray());
            }
            catch (Exception e)
            {
                throw new DataProcessingException(e.Message);
            }

            try
            {
                string[] lines = StringyfyyResults(
                    result,
                    sourceData.StatementColumnCount,
                    sourceData.TransactionColumnCount,
                    reportCsvDelimiter);

                Csv.IO.CsvFileWriter.Write(lines, reportFile);
            }
            catch (Exception e)
            {
                throw new ReportFileWriteException(e.Message);
            }
        }

        private static SourceData ReadSourceData(
            FileInfo statementFile,
            FileInfo transactionsFile,
            char csvDelimiter)
        {
            string[][] statementRows = ReadCsvFile(statementFile, csvDelimiter);
            string[][] transactionRows = ReadCsvFile(transactionsFile, csvDelimiter);

            if (!statementRows.Any()) throw new Exception("Statement file is empty");
            if (!transactionRows.Any()) throw new Exception("Transactions file is empty");

            // want to make sure all rows have equal number of columns. otherwise, it might get tricky down the line
            int transactionColumnCount = transactionRows.First().Length;
            if (!transactionRows.All(row => row.Length == transactionColumnCount))
                throw new Exception("All transaction rows must have the same number of columns");

            int statementColumnCount = statementRows.First().Length;
            if (!statementRows.All(row => row.Length == statementColumnCount))
                throw new Exception("All statement rows must have the same number of columns");

            var transactions = transactionRows.Select(TransactionDetailsParser.Parse);
            var statementEntries = statementRows.Select(StatementEntryParser.Parse);

            return new SourceData
            {
                Transactions = transactions.ToArray(),
                TransactionColumnCount = transactionColumnCount,
                StatementEntries = statementEntries.ToArray(),
                StatementColumnCount = statementColumnCount
            };
        }

        private static string[][] ReadCsvFile(FileInfo file, char delimiter)
        {
            return Csv.IO.CsvFileReader.Read(file, line => Csv.RowParser.Parse(line, delimiter));
        }

        private static string[] StringyfyyResults(
            ComparisonResult<StatementEntryWithSourceData, TransactionDetailsWithSourceData> result,
            int statementColumnCount,
            int transactionColumnCount,
            char csvDelimiter)
        {
            var transactionAndStatementSeparatorColumns = new string[] { "" };
            var blankStatementRow = new string[statementColumnCount];
            var blankTransactionRow = new string[transactionColumnCount];

            var allRows = DataCombiner.GetDataRows(
                result.Matches,
                result.UnmatchedStatementEntries,
                result.UnmatchedTransactions,
                transactionAndStatementSeparatorColumns,
                blankStatementRow,
                blankTransactionRow);

            return allRows
                .Select(row => Csv.RowStringifier.Stringifififiify(row, csvDelimiter))
                .ToArray();
        }
    }
}