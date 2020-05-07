using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class Parameter
    {
        private const string Source1Key = "--source1";
        private const string Source2Key = "--source2";
        private const string ReportFileKey = "--output";
        private const string Command_ExpenseReport = "exp-report";
        private const string Command_Generic = "generic";

        public static Parameters GetParametersOrThrow(IDictionary<string, string> args)
        {
            var command = GetCommand(args);

            string expenseReportFilePath;
            if (!args.TryGetValue(Source2Key, out expenseReportFilePath))
                throw new Exception("A source file 2 must be specified");

            string statementFilePath;
            if (!args.TryGetValue(Source1Key, out statementFilePath))
                throw new Exception("A source file 1 must be specified");

            string reportFilePath;
            if (!args.TryGetValue(ReportFileKey, out reportFilePath))
                throw new Exception("A comparison report file must be specified");

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
            if (args.ContainsKey(Command_ExpenseReport))
                return Command.ExpenseReport;
            if (args.ContainsKey(Command_Generic))
                return Command.Generic;

            throw new Exception(
                $"No suitable command has been specified. It must be one of: {Command_ExpenseReport}, {Command_Generic}");
        }
    }
}