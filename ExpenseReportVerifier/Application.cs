using Andy.ExpenseReport.Comparison;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Cmd
{
    public static class Application
    {
        public static void Go(
            FileInfo statementFile,
            FileInfo transactionsFile,
            FileInfo reportFile,
            ApplicationSettings settings)
        {
            SourceData sourceData;
            try
            {
                sourceData = ReadSourceData(
                    statementFile,
                    transactionsFile,
                    settings.StatementCsvFile,
                    settings.TransactionsCsvFile);
            }
            catch (Exception e)
            {
                throw new SourceDataReadException(e);
            }

            var comparer = new CollectionComparer(
                new MatchFinder(
                    new ItemComparer(
                        new MerchantNameComparer())));

            ComparisonResult<StatementEntryWithSourceData, TransactionDetailsWithSourceData> result;
            try
            {
                result = comparer.Compare(
                    sourceData.StatementEntries,
                    sourceData.Transactions.ToArray());
            }
            catch (Exception e)
            {
                throw new DataProcessingException(e);
            }

            var stringyfyer = new Csv.RowStringifier(
                new Csv.ValueEncoder());

            try
            {
                string[] lines = StringyfyyResults(
                    result,
                    sourceData.StatementColumnCount,
                    sourceData.TransactionColumnCount,
                    settings.OutputCsvDelimiter,
                    stringyfyer);

                Csv.IO.CsvFileWriter.Write(lines, reportFile);
            }
            catch (Exception e)
            {
                throw new ReportFileProductionException(e);
            }
        }

        private static SourceData ReadSourceData(
            FileInfo statementFile,
            FileInfo transactionsFile,
            StatementCsvFileSettings statementFileSettings,
            TransactionCsvFileSettings transactionDetailsFileSettings)
        {
            int statementColumnCount;
            var statementRows = ReadAndValidateRowsFromFile(
                    statementFile,
                    statementFileSettings.Delimiter,
                    out statementColumnCount);

            int transactionColumnCount;
            var transactionRows = ReadAndValidateRowsFromFile(
                    transactionsFile,
                    transactionDetailsFileSettings.Delimiter,
                    out transactionColumnCount);

            var transactionRowParser = new TransactionDetailsParser(transactionDetailsFileSettings.ColumnIndexes);
            var statementRowParser = new StatementEntryParser(statementFileSettings.ColumnIndexes);

            var transactions = transactionRows.Select(transactionRowParser.Parse);
            var statementEntries = statementRows.Select(statementRowParser.Parse);

            return new SourceData
            {
                Transactions = transactions.ToArray(),
                TransactionColumnCount = transactionColumnCount,
                StatementEntries = statementEntries.ToArray(),
                StatementColumnCount = statementColumnCount
            };
        }

        private static string[][] ReadAndValidateRowsFromFile(
            FileInfo statementFile,
            char delimiter,
            out int columnCount)
        {
            string[][] statementRows = ReadRowsFromCsvFile(
                statementFile,
                delimiter);

            if (!statementRows.Any()) throw new Exception("The has no CSV content");

            // want to make sure all rows have equal number of columns. otherwise, it could get tricky down the line
            columnCount = statementRows.First().Length;

            int colCount = columnCount;
            if (!statementRows.All(row => row.Length == colCount))
                throw new Exception("All rows in a CSV file must have the same number of columns");

            return statementRows;
        }

        private static string[][] ReadRowsFromCsvFile(FileInfo file, char delimiter)
        {
            return Csv.IO.CsvFileReader.Read(file, line => Csv.RowParser.Parse(line, delimiter));
        }

        private static string[] StringyfyyResults(
            ComparisonResult<StatementEntryWithSourceData, TransactionDetailsWithSourceData> result,
            int statementColumnCount,
            int transactionColumnCount,
            char csvDelimiter,
            Csv.IRowStringifier stringyfier)
        {
            var transactionAndStatementSeparatorColumns = new string[] { "" };
            var blankStatementRow = new string[statementColumnCount];
            var blankTransactionRow = new string[transactionColumnCount];

            var matchRows = result.Matches.Select(
                x => new Tuple<string[], string[]>(
                    x.Item1.SourceData,
                    x.Item2.SourceData));

            var allRows = ResultAggregation.GetDataRows(
                matchRows,
                result.UnmatchedStatementEntries.Select(x => x.SourceData),
                result.UnmatchedTransactions.Select(x => x.SourceData),
                transactionAndStatementSeparatorColumns,
                blankStatementRow,
                blankTransactionRow);

            return allRows
                .Select(row => stringyfier.Stringifififiify(row, csvDelimiter))
                .ToArray();
        }
    }
}