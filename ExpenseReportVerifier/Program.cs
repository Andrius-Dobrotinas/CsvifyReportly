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

            var csvRowParser = new Csv.RowParser();
            var csvFileReader = new Csv.CsvFileReader();

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

            var transactionAndStatementSeparatorColumns = new string[] { "" };
            var blankStatementRow = new string[statementColumnCount];
            var blankTransactionRow = new string[transactionColumnCount];

            var allRows = DataCombiner.GetDataRows(
                results.Matches,
                results.UnmatchedStatementEntries,
                results.UnmatchedTransactions,
                transactionAndStatementSeparatorColumns,
                blankStatementRow,
                blankTransactionRow);

            var lines = allRows
                .Select(row => Csv.RowStringifier.Stringifififiify(row, ','))
                .ToArray();

            Csv.IO.CsvFileWriter.Write(lines, reportFile);

            Console.WriteLine("Done");
        }

        private static string[][] ReadCsvFile(Csv.CsvFileReader csvReader, Csv.RowParser parser, FileInfo file, char delimiter)
        {
            return csvReader.Read(file, line => parser.Parse(line, delimiter));
        }
    }
}