using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport
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
                throw new SourceDataReadException(e.Message);
            }

            var matcher = new CollectionComparer(
                new MatchFinder(
                    new ItemComparer(
                        new MerchantComparer())));

            ComparisonResult<StatementEntryWithSourceData, TransactionDetailsWithSourceData> result;
            try
            {
                result = matcher.Compare(sourceData.StatementEntries, sourceData.Transactions.ToArray());
            }
            catch (Exception e)
            {
                throw new DataProcessingException(e.Message);
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
                throw new ReportFileWriteException(e.Message);
            }
        }

        private static SourceData ReadSourceData(
            FileInfo statementFile,
            FileInfo transactionsFile,
            StatementCsvFileSettings statementFileSettings,
            TransactionCsvFileSettings transactionDetailsFileSettings)
        {
            string[][] statementRows = ReadCsvFile(
                statementFile,
                statementFileSettings.Delimiter);

            string[][] transactionRows = ReadCsvFile(
                transactionsFile,
                transactionDetailsFileSettings.Delimiter);

            if (!statementRows.Any()) throw new Exception("Statement file is empty");
            if (!transactionRows.Any()) throw new Exception("Transactions file is empty");

            // want to make sure all rows have equal number of columns. otherwise, it might get tricky down the line
            int transactionColumnCount = transactionRows.First().Length;
            if (!transactionRows.All(row => row.Length == transactionColumnCount))
                throw new Exception("All transaction rows must have the same number of columns");

            int statementColumnCount = statementRows.First().Length;
            if (!statementRows.All(row => row.Length == statementColumnCount))
                throw new Exception("All statement rows must have the same number of columns");

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

        private static string[][] ReadCsvFile(FileInfo file, char delimiter)
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

            var allRows = DataCombiner.GetDataRows(
                result.Matches,
                result.UnmatchedStatementEntries,
                result.UnmatchedTransactions,
                transactionAndStatementSeparatorColumns,
                blankStatementRow,
                blankTransactionRow);

            return allRows
                .Select(row => stringyfier.Stringifififiify(row, csvDelimiter))
                .ToArray();
        }
    }
}