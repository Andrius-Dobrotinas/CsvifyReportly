using Andy.Csv.Transformation.Row.Document.Cmd.Conguration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    class Program
    {
        private const string settingsFileName = "settings.json";

        static int Main(string[] args)
        {
            Parameters parameters;
            try
            {
                var arguments = Andy.Cmd.ArgumentParser.ParseArguments(args);
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
                settings = Andy.Cmd.JasonFileParser.ParseContents<Settings>(
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
                var transformers = Profile.GetTransformerChain(settings, parameters.ProfileName);

                var rewriter = new CsvStreamTransformer(
                    new RowStringifier(
                        new ValueEncoder()),
                    transformers,
                    new IO.CsvDocumentReader(
                        new IO.CsvStreamParser(
                            new IO.RowReader(
                                new RowParser(settings.CsvDelimiter)),
                            new IO.StreamReaderFactory(),
                            new IO.StreamReaderPositionReporter())));

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

        private static void Go(CsvStreamTransformer rewriter, FileInfo inputFile, FileInfo outputFile, char delimiter)
        {
            using (var source = inputFile.OpenRead())
            using (var output = outputFile.OpenWrite())
            using (var result = rewriter.Go(source, delimiter))
                result.CopyTo(output);
        }
    }
}