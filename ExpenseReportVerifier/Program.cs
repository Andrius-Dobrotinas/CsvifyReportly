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

            var csvRowParser = new RowParser();
            var csvFileReader = new CsvFileReader();

            string[][] statementRows = ReadCsvFile(csvFileReader, csvRowParser, statementFile, delimiter);
            string[][] transactionRows = ReadCsvFile(csvFileReader, csvRowParser, expenseReportFile, delimiter);

            var transactions = transactionRows.Select(TransactionDetailsParser.Parse);
            var statement = statementRows.Select(StatementEntryParser.Parse);

            var matcher = new CollectionComparer(
                new MatchFinder(
                    new ItemComparer(
                        new MerchantComparer())));

            var results = matcher.Compare(statement, transactions.ToArray());
        }

        private static string[][] ReadCsvFile(CsvFileReader csvReader, RowParser parser, FileInfo file, char delimiter)
        {
            return csvReader.Read(file, line => parser.Parse(line, delimiter));
        }
    }
}