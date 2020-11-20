using Andy.ExpenseReport.Verifier.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Verifier.Custom.Cmd
{
    public static class ComparerBuilder
    {
        private static (Comparison.Csv.ComparerFactory<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData> comparerFactory,
            Comparison.Csv.Statement.StatementEntryParserFactory entry1ParserFactory,
            Comparison.Csv.Statement.Bank.ExpenseReportEntryParserFactory entry2ParserFactory)
            BuildBankStatementComparer(Settings.ExpenseReportComparisonSettings sourceSettings)
        {
            var entry1ParserFactory = new Comparison.Csv.Statement.StatementEntryParserFactory(
                sourceSettings.StatementFile.ColumnNames,
                sourceSettings.StatementFile.DateFormat);

            var entry2ParserFactory = new Comparison.Csv.Statement.Bank.ExpenseReportEntryParserFactory(
                sourceSettings.ExpenseReportFile.ColumnNames,
                sourceSettings.ExpenseReportFile.DateFormat);

            var collectionComparer = new Comparison.CollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                new Comparison.MatchFinder<
                    Comparison.Csv.Statement.StatementEntryWithSourceData,
                    Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                    new Comparison.Statement.Bank.ItemComparer(
                        new Comparison.Statement.Bank.MerchantNameComparer(
                            new Comparison.Statement.Bank.MerchanNameVariationComparer(
                                sourceSettings.MerchantNameMap)),
                        new Comparison.Statement.Bank.InvertedAmountComparer(),
                        new Comparison.Statement.Bank.TolerantDateComparer(
                            sourceSettings.DateTolerance,
                            sourceSettings.Direction))));

            var orderedCollectionComparer = new Comparison.Csv.Statement.OrderedCollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(collectionComparer);

            var comparerFactory = new Comparison.Csv.ComparerFactory<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                    orderedCollectionComparer);

            return (comparerFactory, entry1ParserFactory, entry2ParserFactory);
        }

        public static ReportingFileComparer BuildFileComparer(
            Settings settings,
            Tuple<char, char> csvDelimiters)
        {
            var (comparerFactory, entry1ParserFactory, entry2ParserFactory) = BuildBankStatementComparer(settings.Source);

            var multiTransformers1 = FileComparerBuilder.GetTransformerChain(
                settings.TransformationProfiles, 
                settings.Source.StatementFile.TransformationProfileName);
            var multiTransformers2 = FileComparerBuilder.GetTransformerChain(
                settings.TransformationProfiles, 
                settings.Source.ExpenseReportFile.TransformationProfileName);

            return FileComparerBuilder.BuildFileComparer(
                comparerFactory,
                entry1ParserFactory,
                entry2ParserFactory,
                multiTransformers1,
                multiTransformers2,
                csvDelimiters.Item1,
                csvDelimiters.Item2);
        }
    }
}