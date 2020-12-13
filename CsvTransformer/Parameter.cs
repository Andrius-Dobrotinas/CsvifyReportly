using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public static class Parameter
    {
        public static class Keys
        {
            public const string Source = "--source";
            public const string OutputFile = "--result";
            public const string ProfileName = "--profile";
            public const string Help = "--help";
        }

        public static Parameters GetParametersOrThrow(IDictionary<string, string> args)
        {
            string sourceFile;
            if (!args.TryGetValue(Keys.Source, out sourceFile))
                throw new Exception(@$"A source file must be specified with ""{Keys.OutputFile}"" parameter");

            string reportFilePath;
            if (!args.TryGetValue(Keys.OutputFile, out reportFilePath))
                throw new Exception(@$"An output file must be specified with ""{Keys.Source}"" parameter");

            string profileName;
            args.TryGetValue(Keys.ProfileName, out profileName);

            return new Parameters
            {
                SourceFile = new FileInfo(sourceFile),
                ResultFile = new FileInfo(reportFilePath),
                ProfileName = profileName
            };
        }

        public static void PrintInstructions(Action<string> writeLine)
        {
            writeLine("$Parameters:");
            writeLine($"{Keys.ProfileName}=.. to specify a transformation profile");
            writeLine($"{Keys.Source}=.. to specify a source file");
            writeLine($"{Keys.OutputFile}=.. to specify an output file");
            writeLine($"For example:");
            writeLine($@"transform {Keys.ProfileName}=profile1 {Keys.Source}=expense-report.csv {Keys.OutputFile}=""c:\\result.csv""");
        }
    }
}