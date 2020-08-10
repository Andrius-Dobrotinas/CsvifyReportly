using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public static class Parameter
    {
        private static class Keys
        {
            public const string Source = "--source";
            public const string OutputFile = "--output";
            public const string ProfileName = "--profile";
        }

        public static Parameters GetParametersOrThrow(IDictionary<string, string> args)
        {
            string sourceFile;
            if (!args.TryGetValue(Keys.Source, out sourceFile))
                throw new Exception(@$"A source file must be specified with ""{Keys.OutputFile}"" parameter");

            string reportFilePath;
            if (!args.TryGetValue(Keys.OutputFile, out reportFilePath))
                throw new Exception(@$"An output file must be specified with ""{Keys.OutputFile}"" parameter");

            string profileName;
            args.TryGetValue(Keys.ProfileName, out profileName);

            return new Parameters
            {
                SourceFile = new FileInfo(sourceFile),
                ResultFile = new FileInfo(reportFilePath),
                ProfileName = profileName
            };
        }
    }
}