using Andy.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public class CsvReenumerableRowByteStreamReader : ICsvRowByteStreamReader
    {
        private readonly ICsvRowByteStreamReader expensiveReader;

        public CsvReenumerableRowByteStreamReader(ICsvRowByteStreamReader expensiveReader)
        {
            this.expensiveReader = expensiveReader;
        }

        public IEnumerable<string[]> ReadRows(Stream source)
        {
            var enumerable = expensiveReader.ReadRows(source);
            return Smartnumerable.Create<string[]>(enumerable);
        }
    }
}