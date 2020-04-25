using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Verifier.Cmd
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

            Bank.ExpenseReportParameters<
                Comparison.Csv.Bank.StatementEntryColumnIndexes,
                Comparison.Csv.Bank.TransactionDetailsColumnIndexes> settings;
            try
            {
                settings = SettingsReader.ReadSettings<
                    Bank.ExpenseReportParameters<
                        Comparison.Csv.Bank.StatementEntryColumnIndexes,
                        Comparison.Csv.Bank.TransactionDetailsColumnIndexes>>(
                    new FileInfo(settingsFileName));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("There's a problem with a settings file:");
                Console.Error.WriteLine(e.Message);
                return -50;
            }

            try
            {
                var comparer = BuildBankStatementComparer(settings);
                var fileComparer = BuildFileComparer(comparer);

                fileComparer.CompareAndWriteReport(
                    parameters.StatementFile,
                    parameters.TransactionFile,
                    parameters.ComparisonReportFile,
                    settings);
            }
            catch (CsvStreamComparisonException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.ActualException?.Message);

                return ResolveReturnCode(e);
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

        private static int ResolveReturnCode(CsvStreamComparisonException exception)
        {
            if (exception is SourceDataReadException)
                return -100;
            if (exception is ReportProductionException)
                return -300;
            if (exception is DataComparisonException)
                return -200;

            return -1;
        }

        private static IDictionary<string, string> ParseArguments(string[] args)
        {
            var argParser = new ArgumentParser('=');

            return args.Select(argParser.ParseArgument)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value);
        }

        private static Comparison.Csv.Comparer<
                Comparison.Csv.Bank.StatementEntryWithSourceData,
                Comparison.Csv.Bank.TransactionDetailsWithSourceData>
            BuildBankStatementComparer(
                Bank.ExpenseReportParameters<
                    Comparison.Csv.Bank.StatementEntryColumnIndexes,
                    Comparison.Csv.Bank.TransactionDetailsColumnIndexes> settings)
        {
            var item1Parser = new Comparison.Csv.Bank.StatementEntryParser(settings.StatementCsvFile.ColumnIndexes);
            var item2Parser = new Comparison.Csv.Bank.TransactionDetailsParser(settings.TransactionsCsvFile.ColumnIndexes);

            var collectionComparer = new Comparison.CollectionComparer<
                Comparison.Csv.Bank.StatementEntryWithSourceData,
                Comparison.Csv.Bank.TransactionDetailsWithSourceData>(
                new Comparison.Statement.Bank.MatchFinder<
                    Comparison.Csv.Bank.StatementEntryWithSourceData,
                    Comparison.Csv.Bank.TransactionDetailsWithSourceData>(
                    new Comparison.Statement.Bank.ItemComparer(
                        new Comparison.Statement.Bank.MerchantNameComparer(
                            new Comparison.Statement.Bank.MerchanNameVariationComparer(
                                settings.MerchantNameMap)))));

            var comparer = new Comparison.Csv.Comparer<
                Comparison.Csv.Bank.StatementEntryWithSourceData,
                Comparison.Csv.Bank.TransactionDetailsWithSourceData>(
                    collectionComparer,
                    item1Parser,
                    item2Parser);

            return comparer;
        }

        private static ReportingFileComparer<TItem1, TItem2> BuildFileComparer<TItem1, TItem2>(
             Comparison.Csv.IComparer<TItem1, TItem2> comparer)
        {
            return new ReportingFileComparer<TItem1, TItem2>(
                    new ReportingComparer<TItem1, TItem2>(
                            comparer));
        }
    }
}