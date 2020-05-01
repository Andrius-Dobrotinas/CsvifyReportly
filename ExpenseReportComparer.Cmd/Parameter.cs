using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class Parameter
    {
        private const string ExpenseReportFileKey = "--expenseReport";
        private const string StatementFileKey = "--statements";
        private const string ReportFileKey = "--output";
        private const string Command_Bank = "bank";
        private const string Command_PayPal = "paypal";

        public static Parameters GetParametersOrThrow(IDictionary<string, string> args)
        {
            var command = GetCommand(args);

            string expenseReportFilePath;
            if (!args.TryGetValue(ExpenseReportFileKey, out expenseReportFilePath))
                throw new Exception("A transactions file must be specified");

            string statementFilePath;
            if (!args.TryGetValue(StatementFileKey, out statementFilePath))
                throw new Exception("A statement file must be specified");

            string reportFilePath;
            if (!args.TryGetValue(ReportFileKey, out reportFilePath))
                throw new Exception("A report file must be specified");

            return new Parameters
            {
                Command = command,
                ExpenseReportFile = new FileInfo(expenseReportFilePath),
                StatementFile = new FileInfo(statementFilePath),
                ComparisonReportFile = new FileInfo(reportFilePath)
            };
        }

        private static Command GetCommand(IDictionary<string, string> args)
        {
            if (args.ContainsKey(Command_Bank))
                return Command.Bank;
            if (args.ContainsKey(Command_PayPal))
                return Command.PayPal;

            throw new Exception(
                $"No suitable command has been specified. It must be one of: {Command_Bank}, {Command_PayPal}");
        }
    }
}