using Andy.ExpenseReport.Comparison.Csv.Statement;
using Andy.ExpenseReport.Comparison.Csv.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class Settings
    {
        public ExpenseReportComparisonSettings ExpenseReport { get; set; }
        public GenericComparisonSettings Generic { get; set; }
        public char OutputCsvDelimiter { get; set; }

        public class ExpenseReportComparisonSettings
        {
            public CsvFileSettings<StatementEntryColumnIndexes> StatementFile { get; set; }
            public CsvFileSettings<ExpenseReportEntryColumnIndexes> ExpenseReportFile { get; set; }
            public IDictionary<string, string[]> MerchantNameMap { get; set; }
        }

        public class GenericComparisonSettings
        {
            public CsvFileSettings<StatementEntryColumnIndexes> StatementFile1 { get; set; }
            public CsvFileSettings<StatementEntryColumnIndexes> StatementFile2 { get; set; }
        }

        public class CsvFileSettings<TColumnIndexMap>
            where TColumnIndexMap : class
        {
            public TColumnIndexMap ColumnIndexes { get; set; }
            public char Delimiter { get; set; }
            public string DateFormat { get; set; }
        }
    }
}