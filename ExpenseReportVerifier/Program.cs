using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport
{
    class Program
    {
        private const string settingsFileName = "settings.json";

        static int Main(string[] args)
        {
            Parameters parameters;
            try
            {
                var arguments = ParseArguments(args);
                parameters = Parameter.GetParametersOrThrow(arguments);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("There's a problem with command parameters:");
                Console.Error.WriteLine(e.Message);
                return -2;
            }

            ApplicationSettings settings;
            try
            {
                settings = ApplicationSettings.ReadSettings(new FileInfo(settingsFileName));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("There's a problem with a settings file:");
                Console.Error.WriteLine(e.Message);
                return -50;
            }

            try
            {
                Act(
                    parameters.StatementFile,
                    parameters.TransactionFile,
                    parameters.ComparisonReportFile,
                    settings);
            }
            catch (ConsoleApplicationLevelException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.ExceptionDetails);

                return e.ReturnCode;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unexpected error:");
                Console.Error.WriteLine(e.Message);

                return -1;
            }

            Console.WriteLine("Done");

            return 0;
        }

        private static void Act(
            FileInfo statementFile,
            FileInfo transactionsFile,
            FileInfo reportFile,
            ApplicationSettings settings)
        {
            SourceData sourceData;
            try
            {
                sourceData = ReadSourceData(
                    statementFile,
                    transactionsFile,
                    settings.StatementCsvFile,
                    settings.TransactionsCsvFile);
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

            var stringyfyer = new Csv.RowStringifier(
                new Csv.ValueEncoder());

            try
            {
                string[] lines = StringyfyyResults(
                    result,
                    sourceData.StatementColumnCount,
                    sourceData.TransactionColumnCount,
                    settings.OutputCsvDelimiter,
                    stringyfyer);

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
            StatementCsvFileSettings statementFileSettings,
            TransactionCsvFileSettings transactionDetailsFileSettings)
        {
            string[][] statementRows = ReadCsvFile(
                statementFile,
                statementFileSettings.Delimiter);

            string[][] transactionRows = ReadCsvFile(
                transactionsFile,
                transactionDetailsFileSettings.Delimiter);

            if (!statementRows.Any()) throw new Exception("Statement file is empty");
            if (!transactionRows.Any()) throw new Exception("Transactions file is empty");

            // want to make sure all rows have equal number of columns. otherwise, it might get tricky down the line
            int transactionColumnCount = transactionRows.First().Length;
            if (!transactionRows.All(row => row.Length == transactionColumnCount))
                throw new Exception("All transaction rows must have the same number of columns");

            int statementColumnCount = statementRows.First().Length;
            if (!statementRows.All(row => row.Length == statementColumnCount))
                throw new Exception("All statement rows must have the same number of columns");

            var transactionRowParser = new TransactionDetailsParser(transactionDetailsFileSettings.ColumnIndexes);
            var statementRowParser = new StatementEntryParser(statementFileSettings.ColumnIndexes);

            var transactions = transactionRows.Select(transactionRowParser.Parse);
            var statementEntries = statementRows.Select(statementRowParser.Parse);

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
            char csvDelimiter,
            Csv.IRowStringifier stringyfier)
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
                .Select(row => stringyfier.Stringifififiify(row, csvDelimiter))
                .ToArray();
        }

        private static IDictionary<string, string> ParseArguments(string[] args)
        {
            var argParser = new CommandLine.ArgumentParser('=');

            return args.Select(argParser.ParseArgument)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value);
        }
    }
}