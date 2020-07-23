using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using System;
using System.Collections.Generic;
using System.Linq;

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
                    item2Parser);

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
                    reportEntryParser);

            return comparer;
        }

        private static ReportingFileComparer BuildFileComparer<TItem1, TItem2>(
             Comparison.Csv.IComparer<TItem1, TItem2> comparer,
             char source1CsvDelimiter,
             char source2CsvDelimiter,
             IEnumerable<Csv.Transformation.Row.Document.IDocumentTransformer> doc1Transformers)
        {
            return new ReportingFileComparer(
                    new ReportingComparer<TItem1, TItem2>(
                            comparer,
                            Build_CsvStreamReader(source1CsvDelimiter),
                            Build_CsvStreamReader(source2CsvDelimiter),
                            new Csv.Transformation.Row.Document.MultiTransformer(doc1Transformers)));
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
                        var multiTransformers1 = GetTransformerChain(settings);

                        var comparer = BuildBankStatementComparer(settings);

                        return BuildFileComparer(
                            comparer,
                            csvDelimiters.Item1,
                            csvDelimiters.Item2,
                            multiTransformers1);
                    }
                case Command.Generic:
                    {
                        var comparer = BuildGenericStatementComparer(settings);
                        var multiTransformers1 = new Csv.Transformation.Row.Document.IDocumentTransformer[0];

                        return BuildFileComparer(
                            comparer,
                            csvDelimiters.Item1,
                            csvDelimiters.Item2,
                            multiTransformers1);
                    }
                default:
                    throw new NotImplementedException($"There's no implementation for command {type.ToString()}");
            }
        }

        private static IEnumerable<Csv.Transformation.Row.Document.IDocumentTransformer> GetTransformerChain(Settings settings)
        {
            return Csv.Transformation.Row.Document.Setup.Profile.GetTransformerChain(settings.TransformationProfiles, settings.ExpenseReport.StatementFileTransformationProfileName);
        }
    }
}