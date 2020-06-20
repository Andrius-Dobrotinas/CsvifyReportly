using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public interface ICsvStreamReader
    {
        string[][] Read(Stream source);
    }

    public class CsvStreamReader : ICsvStreamReader
    {
        private readonly ICsvRowParser rowParser;

        public CsvStreamReader(ICsvRowParser rowParser)
        {
            this.rowParser = rowParser;
        }

        public string[][] Read(Stream source)
        {
            return Read(source, rowParser.Parse);
        }

        /// <summary>
        /// Parses the contents of a file while reading it line by line
        /// </summary>
        /// <param name="parseLine">Function that parses a line read from a file</param>
        public static string[][] Read(Stream source, Func<string, string[]> parseLine)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var results = new List<string[]>();

            using (var reader = new StreamReader(source))
            {
                int rowNumber = 0;

                while (reader.EndOfStream == false)
                {
                    rowNumber++;

                    string[] entry;
                    try
                    {
                        string line = reader.ReadLine();
                        entry = parseLine(line);
                    }
                    catch (Exception e)
                    {
                        throw new RowReadingException(rowNumber, e);
                    }

                    results.Add(entry);
                }
            }

            return results.ToArray();
        }
    }
}