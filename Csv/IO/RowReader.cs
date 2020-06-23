using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.Csv.IO
{
    public interface IRowReader
    {
        string[] ReadNextRow(StreamReader reader);
    }

    public class RowReader : IRowReader
    {
        private readonly ICsvRowParser rowParser;

        public RowReader(ICsvRowParser rowParser)
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