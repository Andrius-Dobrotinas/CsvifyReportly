using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public interface ICsvStreamParser
    {
        /// <summary>
        /// Parses the contents of a stream while reading it line by line.
        /// Returns an enumerable structure of CSV rows (which are themselves arrays of cells).
        /// Implementations are to return enumerables that read the stream only when enumerated
        /// </summary>
        IEnumerable<string[]> Read(Stream source);
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

        public IEnumerable<string[]> Read(Stream source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var reader = streamReaderFactory.Build(source))
            {
                int rowNumber = 0;

                while (streamPositionReporter.IsEndOfStream(reader) == false)
                {
                    rowNumber++;

                    yield return ReadNextRow(reader, rowNumber);
                }
            }
        }

        private string[] ReadNextRow(StreamReader reader, int rowNumber)
        {
            try
            {
                return rowParser.ReadNextRow(reader);
            }
            catch (Exception e)
            {
                throw new RowReadingException(rowNumber, e);
            }
        }
    }
}