using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.Csv.IO
{
    public static class CsvFileWriter
    {
        public static Stream Write(string[] rows)
        {
            var result = new MemoryStream();
            
            using(var writer = new StreamWriter(result))
            {
                foreach(var row in rows)
                {
                    writer.WriteLine(row);
                }
            }

            return result;
        }
    }
}