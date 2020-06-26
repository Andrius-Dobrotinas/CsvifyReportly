using Andy.Csv.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.Csv.IO
{
    public interface ICellByteStreamReader
    {
        /// <summary>
        /// Reads next row from the supplied <paramref name="reader"/> and parses it.
        /// Returns an array of cells that comprise a row.
        /// </summary>
        string[] ReadNextRow(StreamReader reader);
    }

    public class CellByteStreamReader : ICellByteStreamReader
    {
        private readonly IRowParser rowParser;

        public CellByteStreamReader(IRowParser rowParser)
        {
            this.rowParser = rowParser;
        }

        public string[] ReadNextRow(StreamReader reader)
        {
            string line = reader.ReadLine();
            return rowParser.Parse(line);
        }
    }
}