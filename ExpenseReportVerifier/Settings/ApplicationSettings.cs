using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport
{
    public class ApplicationSettings
    {
        public StatementCsvFileSettings StatementCsvFile { get; set; }
        public TransactionCsvFileSettings TransactionsCsvFile { get; set; }
        public char OutputCsvDelimiter { get; set; }

        public static ApplicationSettings ReadSettings(FileInfo settingsFile)
        {
            using (var fs = settingsFile.OpenRead())
            {
                using (var reader = new StreamReader(fs))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationSettings>(reader.ReadToEnd());
                }
            }
        }
    }

    public class StatementCsvFileSettings
    {
        public StatementEntryColumnIndexes ColumnIndexes { get; set; }
        public char Delimiter { get; set; }
    }

    public class TransactionCsvFileSettings
    {
        public TransactionDetailsColumnIndexes ColumnIndexes { get; set; }
        public char Delimiter { get; set; }
    }
}