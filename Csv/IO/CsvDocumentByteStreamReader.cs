using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public interface ICsvDocumentByteStreamReader
    {
        CsvDocument Read(Stream source);
    }

    public class CsvDocumentByteStreamReader : ICsvDocumentByteStreamReader
    {
        private readonly ICsvRowByteStreamReader streamReader;

        public CsvDocumentByteStreamReader(ICsvRowByteStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public CsvDocument Read(Stream source)
        {
            var rows = streamReader.ReadRows(source).ToArray();

            if (rows.Any() == false) return null;

            return new CsvDocument
            {
                HeaderCells = rows.First(),
                ContentRows = rows.Skip(1).ToArray()
            };
        }
    }
}