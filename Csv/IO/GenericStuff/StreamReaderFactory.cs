using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.Csv.IO
{
    public interface IStreamReaderFactory
    {
        StreamReader Build(Stream source);
    }

    public class StreamReaderFactory : IStreamReaderFactory
    {
        public StreamReader Build(Stream source)
        {
            return new StreamReader(source);
        }
    }
}