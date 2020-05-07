using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class Parameter
    {
        private static class Keys
        {
            public const string Source1 = "--source1";
            public const string Source2 = "--source2";
            public const string ReportFile = "--output";

            internal static class Command
            {
                public const string ExpenseReport = "exp-report";
                public const string Generic = "generic";
            }
        }

        public static Parameters GetParametersOrThrow(IDictionary<string, string> args)
        {
            var command = GetCommand(args);

            string source1File;
            if (!args.TryGetValue(Keys.Source1, out source1File))
                throw new Exception("A source file 1 must be specified");

            string source2File;
            if (!args.TryGetValue(Keys.Source2, out source2File))
                throw new Exception("A source file 2 must be specified");

            string reportFilePath;
            if (!args.TryGetValue(Keys.ReportFile, out reportFilePath))
                throw new Exception("A comparison report file must be specified");

            return new Parameters
            {
                Command = command,
                Source1File = new FileInfo(source1File),
                Source2File = new FileInfo(source2File),
                ComparisonReportFile = new FileInfo(reportFilePath)
            };
        }

        private static Command GetCommand(IDictionary<string, string> args)
        {
            if (args.ContainsKey(Keys.Command.ExpenseReport))
                return Command.ExpenseReport;
            if (args.ContainsKey(Keys.Command.Generic))
                return Command.Generic;

            throw new Exception(
                $"No suitable command has been specified. It must be one of: {Keys.Command.ExpenseReport}, {Keys.Command.Generic}");
        }
    }
}