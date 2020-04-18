using Andy.ExpenseReport.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport
{
    class Program
    {
        private const char delimiter = ',';

        static void Main(string[] args)
        {
            var statementFile = new FileInfo(args[1]);
            var expenseReportFile = new FileInfo(args[2]);
            var reportFile = new FileInfo(args[3]);

            var csvRowParser = new RowParser();
            var csvFileReader = new CsvFileReader();

            string[][] statementRows = ReadCsvFile(csvFileReader, csvRowParser, statementFile, delimiter);
            string[][] transactionRows = ReadCsvFile(csvFileReader, csvRowParser, expenseReportFile, delimiter);

            if (!statementRows.Any()) throw new Exception("Statement file is empty");
            if (!transactionRows.Any()) throw new Exception("Transactions file is empty");

            // want to make sure all rows have equal number of columns. otherwise, it might get tricky down the line
            int transactionColumnCount = transactionRows.First().Length;
            if (!transactionRows.All(row => row.Length == transactionColumnCount))
                throw new Exception("All transaction rows must have the same number of columns");

            int statementColumnCount = statementRows.First().Length;
            if (!statementRows.All(row => row.Length == statementColumnCount))
                throw new Exception("All statement rows must have the same number of columns");

            var transactions = transactionRows.Select(TransactionDetailsParser.Parse);
            var statement = statementRows.Select(StatementEntryParser.Parse);

            var matcher = new CollectionComparer(
                new MatchFinder(
                    new ItemComparer(
                        new MerchantComparer())));

            var results = matcher.Compare(statement, transactions.ToArray());

            var matchRows = results.Matches.Select(
                x => new Tuple<string[], string[]>(
                    x.Item1.SourceData,
                    x.Item2.SourceData));

            var blankStatementRow = new string[statementColumnCount];
            var blankTransactionRow = new string[transactionColumnCount];

            var unmatchedStatementRows = results.UnmatchedStatementEntries.Select(
                row => new Tuple<string[], string[]>(
                    row.SourceData,
                    blankTransactionRow));

            var unmatchedTransactionRows = results.UnmatchedTransactions.Select(
                row => new Tuple<string[], string[]>(
                    blankStatementRow,
                    row.SourceData));

            var allRowPairs = matchRows
                .Concat(unmatchedStatementRows)
                .Concat(unmatchedTransactionRows);

            var stringyfyer = new RowStringifier();

            var singleColumnRow = new string[] { "" };

            var allRows = allRowPairs.Select(pair => JoinTwoRows(pair, singleColumnRow))
                .ToArray();

            var lines = allRows
                .Select(row => stringyfyer.Stringifififiify(row, ','))
                .ToArray();

            Csv.IO.CsvFileWriter.Write(lines, reportFile);

            Console.WriteLine("Done");
        }

        private static string[] JoinTwoRows(Tuple<string[], string[]> rowPair, string[] separator)
        {
            return rowPair.Item1
                .Concat(separator)
                .Concat(rowPair.Item2)
                .ToArray();
        }

        private static string[][] ReadCsvFile(CsvFileReader csvReader, RowParser parser, FileInfo file, char delimiter)
        {
            return csvReader.Read(file, line => parser.Parse(line, delimiter));
        }
    }
}