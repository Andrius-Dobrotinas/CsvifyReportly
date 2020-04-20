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

            return new Parameters
            {
                TransactionFile = new FileInfo(transactionFilePath),
                StatementFile = new FileInfo(statementFilePath),
                ComparisonReportFile = new FileInfo(reportFilePath)
            };
        }
    }
}