using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Csv.IO
{
    public static class CsvFileWriter
    {
        public static void Write(string[] rows, FileInfo file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            using (var fs = file.Create())
            {
                using(var writer = new StreamWriter(fs))
                {
                    foreach(var row in rows)
                    {
                        writer.WriteLine(row);
                    }
                }
            }
        }
    }
}