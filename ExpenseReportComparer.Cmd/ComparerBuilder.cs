using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class ComparerBuilder
    {
        private static Comparison.Csv.Comparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>
            BuildBankStatementComparer(Settings settings)
        {
            var item1Parser = new Comparison.Csv.Statement.StatementEntryParser(
                settings.ExpenseReport.StatementFile.ColumnIndexes,
                settings.ExpenseReport.StatementFile.DateFormat);

            var item2Parser = new Comparison.Csv.Statement.Bank.ExpenseReportEntryParser(
                settings.ExpenseReport.ExpenseReportFile.ColumnIndexes,
                settings.ExpenseReport.ExpenseReportFile.DateFormat);

            Comparison.Filtering.IFilter<Comparison.Csv.Statement.StatementEntryWithSourceData> item1Filter = settings.ExpenseReport.IgnorePaypal == true
                ? new Comparison.Filtering.Statement.Bank.PayPalTransactionFilter<Comparison.Csv.Statement.StatementEntryWithSourceData>(
                new Comparison.Filtering.Statement.Bank.PaypalTransactionSpotter())
                : null;

            var collectionComparer = new Comparison.CollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                new Comparison.MatchFinder<
                    Comparison.Csv.Statement.StatementEntryWithSourceData,
                    Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                    new Comparison.Statement.Bank.ItemComparer(
                        new Comparison.Statement.Bank.MerchantNameComparer(
                            new Comparison.Statement.Bank.MerchanNameVariationComparer(
                                settings.ExpenseReport.MerchantNameMap)),
                        new Comparison.Statement.Bank.InvertedAmountComparer(),
                        new Comparison.Statement.Bank.TolerantDateComparer(
                            settings.ExpenseReport.DateTolerance))));

            var orderedCollectionComparer = new Comparison.Csv.Statement.OrderedCollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(collectionComparer);

            var comparer = new Comparison.Csv.Comparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                    orderedCollectionComparer,
                    item1Parser,
                    item2Parser,
                    item1Filter);

            return comparer;
        }

        private static Comparison.Csv.Comparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData>
            BuildGenericStatementComparer(Settings settings)
        {
            var statementEntryParser = new Comparison.Csv.Statement.StatementEntryParser(
                settings.Generic.StatementFile1.ColumnIndexes,
                settings.Generic.StatementFile1.DateFormat);

            var reportEntryParser = new Comparison.Csv.Statement.StatementEntryParser(
                settings.Generic.StatementFile2.ColumnIndexes,
                settings.Generic.StatementFile2.DateFormat);

            var collectionComparer = new Comparison.CollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData>(
                new Comparison.MatchFinder<
                    Comparison.Csv.Statement.StatementEntryWithSourceData,
                    Comparison.Csv.Statement.StatementEntryWithSourceData>(
                        new Comparison.Statement.ItemComparer()));

            var orderedCollectionComparer = new Comparison.Csv.Statement.OrderedCollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData>(collectionComparer);

            var comparer = new Comparison.Csv.Comparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData>(
                    orderedCollectionComparer,
                    statementEntryParser,
                    reportEntryParser,
                    null);

            return comparer;
        }

        private static ReportingFileComparer BuildFileComparer<TItem1, TItem2>(
             Comparison.Csv.IComparer<TItem1, TItem2> comparer,
             char source1CsvDelimiter,
             char source2CsvDelimiter)
        {
            return new ReportingFileComparer(
                    new ReportingComparer<TItem1, TItem2>(
                            comparer,
                            Build_CsvStreamReader(source1CsvDelimiter),
                            Build_CsvStreamReader(source2CsvDelimiter)));
        }

        private static Csv.IO.ISafeCsvStreamReader Build_CsvStreamReader(char csvDelimiter)
        {
            return new Csv.IO.SafeCsvStreamReader(
                new Csv.IO.CsvStreamParser(
                    new Csv.IO.RowReader(
                        new Csv.RowParser(csvDelimiter)),
                    new Csv.IO.StreamReaderFactory(),
                    new Csv.IO.StreamReaderPositionReporter()));
        }

        public static ReportingFileComparer BuildFileComparer(
            Command type,
            Settings settings,
            Tuple<char, char> csvDelimiters)
        {
            switch (type)
            {
                case Command.ExpenseReport:
                    {
                        var comparer = BuildBankStatementComparer(settings);
                        return BuildFileComparer(
                            comparer,
                            csvDelimiters.Item1,
                            csvDelimiters.Item2);
                    }
                case Command.Generic:
                    {
                        var comparer = BuildGenericStatementComparer(settings);
                        return BuildFileComparer(
                            comparer,
                            csvDelimiters.Item1,
                            csvDelimiters.Item2);
                    }
                default:
                    throw new NotImplementedException($"There's no implementation for command {type.ToString()}");
            }
        }
    }
}