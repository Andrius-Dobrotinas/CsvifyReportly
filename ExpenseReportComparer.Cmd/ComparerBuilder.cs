using Andy.ExpenseReport.Comparison.Csv.CsvStream;
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
                settings.Source.StatementFile1.TransformationProfileName);

            var multiTransformers2 = FileComparerBuilder.GetTransformerChain(
                settings.TransformationProfiles,
                settings.Source.StatementFile2.TransformationProfileName);

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