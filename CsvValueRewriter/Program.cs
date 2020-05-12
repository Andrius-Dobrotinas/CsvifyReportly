﻿using System;
using System.IO;

namespace Andy.Csv.Rewrite.Value
{
    class Program
    {
        private const string settingsFileName = "settings.json";

        static int Main(string[] args)
        {
            Parameters parameters;
            try
            {
                var arguments = Cmd.ArgumentParser.ParseArguments(args);
                parameters = Parameter.GetParametersOrThrow(arguments);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("There's a problem with command parameters:");
                Console.Error.WriteLine(e.Message);
                return -2;
            }

            Settings settings;
            try
            {
                settings = Cmd.JasonFileParser.ParseContents<Settings>(
                    new FileInfo(settingsFileName));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("There's a problem with a settings file:");
                Console.Error.WriteLine(e.Message);
                return -50;
            }

            try
            {
                var rewriter = new CsvRewriter(
                    new RowStringifier(
                        new ValueEncoder()),
                    new CsvRowRewriter(
                        settings.TargetColumnIndex,
                        new ValueRewriter(settings.SourceFormat, settings.TargetFormat)));

                Go(rewriter,
                    parameters.SourceFile,
                    parameters.ResultFile,
                    settings.CsvDelimiter);

                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unexpected error:");
                Console.Error.WriteLine(e.Message);

                return -1;
            }
        }

        private static void Go(CsvRewriter rewriter, FileInfo inputFile, FileInfo outputFile, char delimiter)
        {
            using (var source = inputFile.OpenRead())
            using (var output = outputFile.OpenWrite())
            using (var result = rewriter.Go(source, delimiter))
                result.CopyTo(output);
        }
    }
}