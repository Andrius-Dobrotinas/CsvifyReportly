﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public static class CsvFileReader
    {
        /// <summary>
        /// Parses the contents of a file while reading it line by line
        /// </summary>
        /// <param name="parseLine">Function that parses a line read from a file</param>
        public static string[][] Read(FileInfo file, Func<string, string[]> parseLine)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var results = new List<string[]>();

            using (var fs = file.OpenRead())
            using (var reader = new StreamReader(fs))
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