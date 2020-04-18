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

            var parser = new RowParser();
            var csvReader = new CsvFileReader();

            string[][] statementRows = ReadCsv(csvReader, parser, statementFile, delimiter);
            string[][] transactionRows = ReadCsv(csvReader, parser, expenseReportFile, delimiter);

            var transactions = transactionRows.Select(TransactionDetailsParser.Parse);
            var statement = statementRows.Select(StatementEntryParser.Parse);

            var matcher = new CollectionComparer(
                new MatcherFinder(
                    new ItemComparer(
                        new MerchantComparer())));

            var results = matcher.CheckForMatches(statement, transactions.ToArray());
        }

        private static string[][] ReadCsv(CsvFileReader csvReader, RowParser parser, FileInfo file, char delimiter)
        {
            return csvReader.Read(file, line => parser.Parse(line, delimiter));
        }
    }
}