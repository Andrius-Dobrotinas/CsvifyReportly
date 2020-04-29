using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class ComparerBuilder
    {
        public static Comparison.Csv.Comparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.TransactionDetailsWithSourceData>
            BuildBankStatementComparer(Settings settings)
        {
            var item1Parser = new Comparison.Csv.Statement.StatementEntryParser(settings.StatementFile.ColumnIndexes);
            var item2Parser = new Comparison.Csv.Statement.Bank.TransactionDetailsParser(settings.TransactionsFile.ColumnIndexes);

            var collectionComparer = new Comparison.CollectionComparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.TransactionDetailsWithSourceData>(
                new Comparison.MatchFinder<
                    Comparison.Csv.Statement.StatementEntryWithSourceData,
                    Comparison.Csv.Statement.Bank.TransactionDetailsWithSourceData>(
                    new Comparison.Statement.Bank.ItemComparer(
                        new Comparison.Statement.Bank.MerchantNameComparer(
                            new Comparison.Statement.Bank.MerchanNameVariationComparer(
                                settings.MerchantNameMap)))));

            var comparer = new Comparison.Csv.Comparer<
                Comparison.Csv.Statement.StatementEntryWithSourceData,
                Comparison.Csv.Statement.Bank.TransactionDetailsWithSourceData>(
                    collectionComparer,
                    item1Parser,
                    item2Parser);

            return comparer;
        }

        public static ReportingFileComparer BuildFileComparer<TItem1, TItem2>(
             Comparison.Csv.IComparer<TItem1, TItem2> comparer)
        {
            return new ReportingFileComparer(
                    new ReportingComparer<TItem1, TItem2>(
                            comparer));
        }
    }
}