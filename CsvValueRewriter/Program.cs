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
                var rewritersNFilters = GetRewritersAndFilters(settings, parameters);

                var rewriter = new CsvStreamTransformer(
                    new RowStringifier(
                        new ValueEncoder()),
                    rewritersNFilters.Item1,
                    rewritersNFilters.Item2);

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

        private static Tuple<IDocumentTransformer[], Filter.IRowMatchEvaluator[]> GetRewritersAndFilters(
            Settings settings,
            Parameters parameters)
        {
            var csvRewriters = RewriterChain.GetRewriterChain(settings, parameters.RewriterChainName);
            var rowFilters = FilterChain.GetFilterChain(settings, parameters.FilterChainName);
            
            if (!csvRewriters.Any() && !rowFilters.Any())
                throw new Exception("No existing rewriter and filters have been specified");

            return new Tuple<IDocumentTransformer[], Filter.IRowMatchEvaluator[]>(csvRewriters, rowFilters);
        }
    }
}