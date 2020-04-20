using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Cmd
{
    class Program
    {
        private const string settingsFileName = "settings.json";

        static int Main(string[] args)
        {
            Parameters parameters;
            try
            {
                var arguments = ParseArguments(args);
                parameters = Parameter.GetParametersOrThrow(arguments);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("There's a problem with command parameters:");
                Console.Error.WriteLine(e.Message);
                return -2;
            }

            ApplicationSettings settings;
            try
            {
                settings = ApplicationSettings.ReadSettings(new FileInfo(settingsFileName));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("There's a problem with a settings file:");
                Console.Error.WriteLine(e.Message);
                return -50;
            }

            try
            {
                Application.Go(
                    parameters.StatementFile,
                    parameters.TransactionFile,
                    parameters.ComparisonReportFile,
                    settings);
            }
            catch (ConsoleApplicationLevelException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.ExceptionDetails);

                return e.ReturnCode;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unexpected error:");
                Console.Error.WriteLine(e.Message);

                return -1;
            }

            Console.WriteLine("Done");

            return 0;
        }

        private static IDictionary<string, string> ParseArguments(string[] args)
        {
            var argParser = new CommandLine.ArgumentParser('=');

            return args.Select(argParser.ParseArgument)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value);
        }
    }
}