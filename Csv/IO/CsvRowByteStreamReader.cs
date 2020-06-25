using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public interface ICsvRowByteStreamReader
    {
        /// <summary>
        /// Parses the contents of a stream while reading it line by line.
        /// Returns an enumerable structure of CSV rows (which are themselves arrays of cells).
        /// Implementations return enumerables that read the stream only when enumerated.
        /// </summary>
        IEnumerable<string[]> ReadRows(Stream source);
    }

    public class CsvRowByteStreamReader : ICsvRowByteStreamReader
    {
        private readonly ICellByteStreamReader rowReader;
        private readonly IStreamReaderFactory streamReaderFactory;
        private readonly IStreamReaderPositionReporter streamPositionReporter;

        public CsvRowByteStreamReader(ICellByteStreamReader rowReader,
            IStreamReaderFactory streamReaderFactory,
            IStreamReaderPositionReporter streamPositionReporter)
        {
            this.rowReader = rowReader;
            this.streamReaderFactory = streamReaderFactory;
            this.streamPositionReporter = streamPositionReporter;
        }

        public IEnumerable<string[]> ReadRows(Stream source)
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
                return rowReader.ReadNextRow(reader);
            }
            catch (Exception e)
            {
                throw new RowReadingException(rowNumber, e);
            }
        }
    }
}