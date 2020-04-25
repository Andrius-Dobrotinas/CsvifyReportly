﻿using Andy.ExpenseReport.Comparison.Csv.CsvStream;
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
                ExpenseReport.Comparison.Csv.Bank.StatementEntryColumnIndexes,
                ExpenseReport.Comparison.Csv.Bank.TransactionDetailsColumnIndexes> settings;
            try
            {
                settings = SettingsReader.ReadSettings<
                    Bank.ExpenseReportParameters<
                        ExpenseReport.Comparison.Csv.Bank.StatementEntryColumnIndexes,
                        ExpenseReport.Comparison.Csv.Bank.TransactionDetailsColumnIndexes>>(
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
                var item1Parser = new ExpenseReport.Comparison.Csv.Bank.StatementEntryParser(settings.StatementCsvFile.ColumnIndexes);
                var item2Parser = new ExpenseReport.Comparison.Csv.Bank.TransactionDetailsParser(settings.TransactionsCsvFile.ColumnIndexes);  

                var collectionComparer = new ExpenseReport.Comparison.CollectionComparer<
                    ExpenseReport.Comparison.Csv.Bank.StatementEntryWithSourceData,
                    ExpenseReport.Comparison.Csv.Bank.TransactionDetailsWithSourceData>(
                new ExpenseReport.Comparison.Statement.Bank.MatchFinder<
                    ExpenseReport.Comparison.Csv.Bank.StatementEntryWithSourceData,
                    ExpenseReport.Comparison.Csv.Bank.TransactionDetailsWithSourceData>(
                    new ExpenseReport.Comparison.Statement.Bank.ItemComparer(
                        new ExpenseReport.Comparison.Statement.Bank.MerchantNameComparer(
                            new ExpenseReport.Comparison.Statement.Bank.MerchanNameVariationComparer(
                                settings.MerchantNameMap)))));

                var comparer = new ExpenseReport.Comparison.Csv.Comparer<
                    ExpenseReport.Comparison.Csv.Bank.StatementEntryWithSourceData,
                    ExpenseReport.Comparison.Csv.Bank.TransactionDetailsWithSourceData>(
                        collectionComparer,
                        item1Parser,
                        item2Parser);

                var fileComparer = new ReportingFileComparer<
                        ExpenseReport.Comparison.Csv.Bank.StatementEntryWithSourceData,
                        ExpenseReport.Comparison.Csv.Bank.TransactionDetailsWithSourceData>(
                        new ExpenseReport.Comparison.Csv.CsvStream.ReportingComparer<
                            ExpenseReport.Comparison.Csv.Bank.StatementEntryWithSourceData,
                            ExpenseReport.Comparison.Csv.Bank.TransactionDetailsWithSourceData>(
                                comparer));

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
    }
}