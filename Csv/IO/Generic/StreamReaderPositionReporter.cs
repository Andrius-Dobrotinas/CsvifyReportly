using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.Csv.IO
{
    public interface IStreamReaderPositionReporter
    {
        public bool IsEndOfStream(StreamReader streamReader);
    }

    public class StreamReaderPositionReporter : IStreamReaderPositionReporter
    {
        public bool IsEndOfStream(StreamReader streamReader)
        {
            return streamReader.EndOfStream;
        }
    }
}