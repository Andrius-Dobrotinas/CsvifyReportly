﻿using System;
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
            var transactionsFile = new FileInfo(args[2]);
            var reportFile = new FileInfo(args[3]);
            
            var sourceData = ReadSourceData(statementFile, transactionsFile, delimiter);

            var matcher = new CollectionComparer(
                new MatchFinder(
                    new ItemComparer(
                        new MerchantComparer())));

            var result = matcher.Compare(sourceData.StatementEntries, sourceData.Transactions.ToArray());

            string[] lines = StringyfyyResults(
                result,
                sourceData.StatementColumnCount,
                sourceData.TransactionColumnCount);

            Csv.IO.CsvFileWriter.Write(lines, reportFile);

            Console.WriteLine("Done");
        }

        private static SourceData ReadSourceData(
            FileInfo statementFile,
            FileInfo transactionsFile,
            char csvDelimiter)
        {
            string[][] statementRows = ReadCsvFile(statementFile, csvDelimiter);
            string[][] transactionRows = ReadCsvFile(transactionsFile, csvDelimiter);

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
            var statementEntries = statementRows.Select(StatementEntryParser.Parse);

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
            int transactionColumnCount)
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
                .Select(row => Csv.RowStringifier.Stringifififiify(row, ','))
                .ToArray();
        }
    }
}