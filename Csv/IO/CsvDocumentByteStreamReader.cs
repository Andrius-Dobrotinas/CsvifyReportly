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
        private readonly IArrayValueUniquenessChecker arrayValueUniquenessChecker;

        public CsvDocumentByteStreamReader(
            ICsvRowByteStreamReader streamReader,
            IArrayValueUniquenessChecker arrayValueUniquenessChecker)
        {
            this.streamReader = streamReader;
            this.arrayValueUniquenessChecker = arrayValueUniquenessChecker;
        }

        public CsvDocument Read(Stream source)
        {
            var rows = streamReader.ReadRows(source).ToArray();

            if (rows.Any() == false) return null;

            var headerCells = rows.First();
            if (arrayValueUniquenessChecker.HasDuplicates(headerCells))
                throw new StructureException("Column names are not unique");

            return new CsvDocument
            {
                HeaderCells = headerCells,
                ContentRows = rows.Skip(1).ToArray()
            };
        }
    }
}