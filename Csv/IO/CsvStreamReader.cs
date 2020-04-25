using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public static class CsvStreamReader
    {
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
                while (reader.EndOfStream == false)
                {
                    string line = reader.ReadLine();

                    string[] entry = parseLine(line);

                    results.Add(entry);
                }
            }

            return results.ToArray();
        }
    }
}