using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class ComparerBuilder
    {
        public static Comparison.Csv.Comparer<
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

        public static ReportingFileComparer<TItem1, TItem2> BuildFileComparer<TItem1, TItem2>(
             Comparison.Csv.IComparer<TItem1, TItem2> comparer)
        {
            return new ReportingFileComparer<TItem1, TItem2>(
                    new ReportingComparer<TItem1, TItem2>(
                            comparer));
        }
    }
}