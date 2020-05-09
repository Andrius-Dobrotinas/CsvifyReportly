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

            var collectionComparer = new Comparison.CollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                new Comparison.MatchFinder<
                    Comparison.Csv.Statement.StatementEntryWithSourceData,
                    Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                    new Comparison.Statement.Bank.ItemComparer(
                        new Comparison.Statement.Bank.MerchantNameComparer(
                            new Comparison.Statement.Bank.MerchanNameVariationComparer(
                                settings.ExpenseReport.MerchantNameMap)))));

            var comparer = new Comparison.Csv.Comparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.ExpenseReportEntryWithSourceData>(
                    collectionComparer,
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

            var comparer = new Comparison.Csv.Comparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.StatementEntryWithSourceData>(
                    collectionComparer,
                    statementEntryParser,
                    reportEntryParser);

            return comparer;
        }

        private static ReportingFileComparer BuildFileComparer<TItem1, TItem2>(
             Comparison.Csv.IComparer<TItem1, TItem2> comparer)
        {
            return new ReportingFileComparer(
                    new ReportingComparer<TItem1, TItem2>(
                            comparer));
        }

        public static ReportingFileComparer BuildFileComparer(Command type, Settings settings)
        {
            switch (type)
            {
                case Command.ExpenseReport:
                    {
                        var comparer = BuildBankStatementComparer(settings);
                        return BuildFileComparer(comparer);
                    }
                case Command.Generic:
                    {
                        var comparer = BuildGenericStatementComparer(settings);
                        return BuildFileComparer(comparer);
                    }
                default:
                    throw new NotImplementedException($"There's no implementation for command {type.ToString()}");
            }
        }
    }
}