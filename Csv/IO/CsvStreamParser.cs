using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public interface ICsvStreamParser
    {
        string[][] Read(Stream source);
    }

    public class CsvStreamParser : ICsvStreamParser
    {
        private readonly IRowReader rowParser;
        private readonly IStreamReaderFactory streamReaderFactory;
        private readonly IStreamReaderPositionReporter streamPositionReporter;

        public CsvStreamParser(IRowReader rowParser,
            IStreamReaderFactory streamReaderFactory,
            IStreamReaderPositionReporter streamPositionReporter)
        {
            this.rowParser = rowParser;
            this.streamReaderFactory = streamReaderFactory;
            this.streamPositionReporter = streamPositionReporter;
        }

        /// <summary>
        /// Parses the contents of a file while reading it line by line
        /// </summary>
        /// <param name="parseLine">Function that parses a line read from a file</param>
        public string[][] Read(Stream source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var results = new List<string[]>();

            using (var reader = streamReaderFactory.Build(source))
            {
                int rowNumber = 0;

                while (streamPositionReporter.IsEndOfStream(reader) == false)
                {
                    rowNumber++;

                    string[] entry;
                    try
                    {
                        entry = rowParser.ReadNextRow(reader);
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