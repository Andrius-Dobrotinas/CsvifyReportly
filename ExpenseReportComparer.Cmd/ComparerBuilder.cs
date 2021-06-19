using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class ComparerBuilder
    {
        private static (Comparison.Csv.ComparerFactory<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData> comparerFactory,
            Comparison.Csv.Statement.StatementEntryParserFactory entry1ParserFactory,
            Comparison.Csv.Statement.StatementEntryParserFactory entry2ParserFactory)
            BuildGenericStatementComparer(Settings.SourceSettings sourceSettings)
        {
            var entry1ParserFactory = new Comparison.Csv.Statement.StatementEntryParserFactory(
                sourceSettings.StatementFile1.ColumnNames,
                sourceSettings.StatementFile1.DateFormat);

            var entry2ParserFactory = new Comparison.Csv.Statement.StatementEntryParserFactory(
                sourceSettings.StatementFile2.ColumnNames,
                sourceSettings.StatementFile2.DateFormat);

            var matcherFinders = sourceSettings.Comparers
                .Select(comparerSettings => comparerSettings.BuildComparer())
                .Select(itemComparer => new Comparison.MatchFinder<
                                                Comparison.Csv.Statement.StatementEntryWithSourceData,
                                                Comparison.Csv.Statement.StatementEntryWithSourceData>(
                                                    itemComparer))
                .ToArray();

            var collectionComparer = new Comparison.CollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData>(
                matcherFinders);

            var orderedCollectionComparer = new Comparison.Csv.Statement.OrderedCollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData>(collectionComparer);

            var comparerFactory = new Comparison.Csv.ComparerFactory<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData>(
                    orderedCollectionComparer);

            return (comparerFactory, entry1ParserFactory, entry2ParserFactory);
        }

        public static ReportingFileComparer BuildFileComparer(
            Settings settings,
            Tuple<char, char> csvDelimiters)
        {
            var (comparerFactory, entry1ParserFactory, entry2ParserFactory) = BuildGenericStatementComparer(settings.Source);

            var multiTransformers1 = FileComparerBuilder.GetTransformerChain(
                settings.TransformationProfiles,
                settings.Source.StatementFile1.TransformationProfileName,
                settings);

            var multiTransformers2 = FileComparerBuilder.GetTransformerChain(
                settings.TransformationProfiles,
                settings.Source.StatementFile2.TransformationProfileName,
                settings);

            return FileComparerBuilder.BuildFileComparer(
                comparerFactory,
                entry1ParserFactory,
                entry2ParserFactory,
                multiTransformers1,
                multiTransformers2,
                csvDelimiters.Item1,
                csvDelimiters.Item2);
        }

        //private static Comparison.IItemComparer<TStatementEntry, TStatementEntry> BuildItemComparer<TStatementEntry>(Settings.SourceSettings sourceSettings)
        //    where TStatementEntry : Comparison.Statement.StatementEntry
        //{
        //    return ComparerSettings

        //    return new Comparison.Statement.ItemComparer(
        //                    // TODO: read settings and take either this or the straight-forward one
        //                    new Comparison.Statement.MerchantNameComparer(
        //                        new Comparison.Statement.Bank.MerchanNameVariationComparer(sourceSettings.MerchantNameMap),
        //                        new Comparison.Statement.StraighforwardDetailsComparer()),
        //                    new Comparison.Statement.Bank.InvertedAmountComparer(),
        //                    new Comparison.Statement.Bank.TolerantDateComparer(5) // TODO: get the value from the settings
        //                    );
        //}
    }
}