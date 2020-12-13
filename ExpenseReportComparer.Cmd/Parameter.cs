using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class Parameter
    {
        public static class Keys
        {
            public const string Source1 = "--source1";
            public const string Source2 = "--source2";
            public const string ReportFile = "--result";
            public const string Help = "--help";
        }

        public static Parameters GetParametersOrThrow(IDictionary<string, string> args)
        {
            string source1File;
            if (!args.TryGetValue(Keys.Source1, out source1File))
                throw new Exception(@$"A source file 1 must be specified with ""{Keys.Source1}"" parameter");

            string source2File;
            if (!args.TryGetValue(Keys.Source2, out source2File))
                throw new Exception(@$"A source file 2 must be specified with ""{Keys.Source2}"" parameter");

            string reportFilePath;
            if (!args.TryGetValue(Keys.ReportFile, out reportFilePath))
                throw new Exception(@$"A comparison report/result file must be specified with ""{Keys.ReportFile}"" parameter");

            return new Parameters
            {
                Source1File = new FileInfo(source1File),
                Source2File = new FileInfo(source2File),
                ComparisonReportFile = new FileInfo(reportFilePath)
            };
        }

        public static void PrintInstructions(Action<string> writeLine)
        {
            writeLine("$Parameters:");
            writeLine($"{Keys.Source1}=.. for source file 1");
            writeLine($"{Keys.Source2}=.. for source file 2");
            writeLine($"{Keys.ReportFile}=.. for the output file");
            writeLine($"For example:");
            writeLine($@"expenses {Keys.Source1}=""c:\\expenses.csv"" {Keys.Source2}=statements.csv {Keys.ReportFile}=""c:\\result.csv""");
        }
    }
}