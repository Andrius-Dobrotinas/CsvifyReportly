using System;
using System.Collections.Generic;
using System.IO;

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
}