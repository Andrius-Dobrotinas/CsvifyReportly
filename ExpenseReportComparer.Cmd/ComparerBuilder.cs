﻿using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class ComparerBuilder
    {
        private static (Comparison.Csv.ComparerFactory<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData> comparerFactory,
            Comparison.Csv.Statement.StatementEntryParserFactory item1ParserFactory,
            Comparison.Csv.Statement.Bank.ExpenseReportEntryParserFactory item2ParserFactory)
            BuildBankStatementComparer(Settings settings)
        {
            var item1ParserFactory = new Comparison.Csv.Statement.StatementEntryParserFactory(
                settings.ExpenseReport.StatementFile.ColumnNames,
                settings.ExpenseReport.StatementFile.DateFormat);

            var item2ParserFactory = new Comparison.Csv.Statement.Bank.ExpenseReportEntryParserFactory(
                settings.ExpenseReport.ExpenseReportFile.ColumnNames,
                settings.ExpenseReport.ExpenseReportFile.DateFormat);

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

            var comparerFactory = new Comparison.Csv.ComparerFactory<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                    orderedCollectionComparer);

            return (comparerFactory, item1ParserFactory, item2ParserFactory);
        }

        private static (Comparison.Csv.ComparerFactory<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData> comparerFactory,
            Comparison.Csv.Statement.StatementEntryParserFactory item1ParserFactory,
            Comparison.Csv.Statement.StatementEntryParserFactory item2ParserFactory)
            BuildGenericStatementComparer(Settings settings)
        {
            var item1ParserFactory = new Comparison.Csv.Statement.StatementEntryParserFactory(
                settings.Generic.StatementFile1.ColumnNames,
                settings.Generic.StatementFile1.DateFormat);

            var item2ParserFactory = new Comparison.Csv.Statement.StatementEntryParserFactory(
                settings.Generic.StatementFile2.ColumnNames,
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

            var comparerFactory = new Comparison.Csv.ComparerFactory<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData>(
                    orderedCollectionComparer);

            return (comparerFactory, item1ParserFactory, item2ParserFactory);
        }

        private static ReportingFileComparer BuildFileComparer<TItem1, TItem2>(
            Comparison.Csv.IComparerFactory<TItem1, TItem2> comparerFactory,
            Comparison.Csv.Statement.IStatementEntryParserFactory<TItem1> item1ParserFactory,
            Comparison.Csv.Statement.IStatementEntryParserFactory<TItem2> item2ParserFactory,
            IEnumerable<Csv.Transformation.Row.Document.IDocumentTransformer> doc1Transformers,
            IEnumerable<Csv.Transformation.Row.Document.IDocumentTransformer> doc2Transformers,
            char source1CsvDelimiter,
            char source2CsvDelimiter)
        {
            return new ReportingFileComparer(
                    new ReportingComparer<TItem1, TItem2>(
                            Build_CsvStreamReader(source1CsvDelimiter),
                            Build_CsvStreamReader(source2CsvDelimiter),
                            new Csv.Transformation.Row.Document.MultiTransformer(doc1Transformers),
                            new Csv.Transformation.Row.Document.MultiTransformer(doc2Transformers),
                            item1ParserFactory,
                            item2ParserFactory,
                            comparerFactory,
                            new Csv.Transformation.Row.Document.ColumnMapBuilder()));
        }

        private static Csv.IO.ICsvDocumentByteStreamReader Build_CsvStreamReader(char csvDelimiter)
        {
            // todo: i want to reuse some of these instances with the other reader

            return new Csv.IO.CsvDocumentByteStreamReader(
                new Csv.IO.RowLengthValidatingCsvRowByteStreamReader(
                    new Csv.IO.CsvReenumerableRowByteStreamReader(
                        new Csv.IO.CsvRowByteStreamReader(
                            new Csv.IO.CellByteStreamReader(
                                new Csv.Serialization.RowParser(csvDelimiter)),
                            new Csv.IO.StreamReaderFactory(),
                            new Csv.IO.StreamReaderPositionReporter()))),
                new Csv.ArrayValueUniquenessChecker());
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
                        var (comparerFactory, item1ParserFactory, item2ParserFactory) = BuildBankStatementComparer(settings);

                        var multiTransformers1 = GetTransformerChain(settings, settings.ExpenseReport.StatementFile.TransformationProfileName);
                        var multiTransformers2 = GetTransformerChain(settings, settings.ExpenseReport.ExpenseReportFile.TransformationProfileName);

                        return BuildFileComparer(
                            comparerFactory,
                            item1ParserFactory,
                            item2ParserFactory,
                            multiTransformers1,
                            multiTransformers2,
                            csvDelimiters.Item1,
                            csvDelimiters.Item2);
                    }
                case Command.Generic:
                    {
                        var (comparerFactory, item1ParserFactory, item2ParserFactory) = BuildGenericStatementComparer(settings);

                        var multiTransformers1 = GetTransformerChain(settings, settings.Generic.StatementFile1.TransformationProfileName);
                        var multiTransformers2 = GetTransformerChain(settings, settings.Generic.StatementFile2.TransformationProfileName);

                        return BuildFileComparer(
                            comparerFactory,
                            item1ParserFactory,
                            item2ParserFactory,
                            multiTransformers1,
                            multiTransformers2,
                            csvDelimiters.Item1,
                            csvDelimiters.Item2);
                    }
                default:
                    throw new NotImplementedException($"There's no implementation for command {type.ToString()}");
            }
        }

        private static IEnumerable<Csv.Transformation.Row.Document.IDocumentTransformer> GetTransformerChain(Settings settings, string profileName)
        {
            return Csv.Transformation.Row.Document.Setup.Profile.GetTransformerChain(settings.TransformationProfiles, profileName);
        }
    }
}