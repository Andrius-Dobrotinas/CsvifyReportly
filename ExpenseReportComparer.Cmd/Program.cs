using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Verifier.Cmd
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
                ReportingFileComparer fileComparer = ComparerBuilder.BuildFileComparer(parameters.Command, settings);

                var delimiters = GetDelimiters(parameters.Command, settings);

                fileComparer.CompareAndWriteReport(
                    parameters.Source1File,
                    parameters.Source2File,
                    parameters.ComparisonReportFile,
                    delimiters.Item1,
                    delimiters.Item2,
                    settings.OutputCsvDelimiter);
            }
            catch (ExpenseReport.Comparison.Csv.InputParsingException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.InnerException?.Message);

                return -667;
            }
            catch (CsvStreamComparisonException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.ActualException?.Message);

                return ResolveReturnCode(e);
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

        private static int ResolveReturnCode(CsvStreamComparisonException exception)
        {
            if (exception is SourceDataReadException)
                return -100;
            if (exception is ReportProductionException)
                return -300;
            if (exception is DataComparisonException)
                return -200;

            return -1;
        }

        private static Tuple<char, char> GetDelimiters(Command type, Settings settings)
        {
            switch (type)
            {
                case Command.ExpenseReport:
                    {
                        return new Tuple<char, char>(
                            settings.ExpenseReport.StatementFile.Delimiter,
                            settings.ExpenseReport.ExpenseReportFile.Delimiter);
                    }

                case Command.Generic:
                    {
                        return new Tuple<char, char>(
                            settings.Generic.StatementFile1.Delimiter,
                            settings.Generic.StatementFile2.Delimiter);
                    }

                default:
                    throw new NotImplementedException($"Type {type.ToString()}");
            }                
        }
    }
}