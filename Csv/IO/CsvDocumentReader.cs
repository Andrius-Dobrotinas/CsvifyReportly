using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public interface ICsvDocumentReader
    {
        CsvDocument Read(Stream source);
    }

    public class CsvDocumentReader : ICsvDocumentReader
    {
        private readonly ICsvStreamParser streamReader;

        public CsvDocumentReader(ICsvStreamParser streamReader)
        {
            this.streamReader = streamReader;
        }

        public CsvDocument Read(Stream source)
        {
            var rows = streamReader.Read(source);

            if (rows.Any() == false) return null;

            return new CsvDocument
            {
                HeaderCells = rows.First(),
                ContentRows = rows.Skip(1).ToArray()
            };
        }
    }
}