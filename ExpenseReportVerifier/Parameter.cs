using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport
{
    public static class Parameter
    {
        private const string TransactionFileKey = "--transactions";
        private const string StatementFileKey = "--statements";
        private const string ReportFileKey = "--output";
        private const string InputCsvDelimiterKey = "--in-delimiter";
        private const string OutputCsvDelimiterKey = "--out-delimiter";

        public static Parameters GetParametersOrThrow(IDictionary<string, string> args)
        {
            string transactionFilePath;
            if (!args.TryGetValue(TransactionFileKey, out transactionFilePath))
                throw new Exception("A transactions file must be specified");

            string statementFilePath;
            if (!args.TryGetValue(StatementFileKey, out statementFilePath))
                throw new Exception("A statement file must be specified");

            string reportFilePath;
            if (!args.TryGetValue(ReportFileKey, out reportFilePath))
                throw new Exception("A report file must be specified");

            string inputCsvDelimiterString;
            if (!args.TryGetValue(InputCsvDelimiterKey, out inputCsvDelimiterString))
                throw new Exception("An input CSV delimiter must be specified");

            char inputCsvDelimiter;
            if (!char.TryParse(inputCsvDelimiterString, out inputCsvDelimiter))
                throw new Exception("Input CSV delimiter must be a single character");

            string outputCsvDelimiterString;
            if (!args.TryGetValue(OutputCsvDelimiterKey, out outputCsvDelimiterString))
                throw new Exception("An input CSV delimiter must be specified");

            char outputCsvDelimiter;
            if (!char.TryParse(outputCsvDelimiterString, out outputCsvDelimiter))
                throw new Exception("Output CSV delimiter must be a single character");

            return new Parameters
            {
                TransactionFile = new FileInfo(transactionFilePath),
                StatementFile = new FileInfo(statementFilePath),
                ReportFile = new FileInfo(reportFilePath),
                InputCsvDelimiter = inputCsvDelimiter,
                OutputCsvDelimiter = outputCsvDelimiter
            };
        }
    }
}