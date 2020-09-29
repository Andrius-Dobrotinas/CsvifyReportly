using Andy.ExpenseReport.Comparison.Csv.Statement;
using Andy.ExpenseReport.Comparison.Csv.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class Settings
    {
        public SourceSettings Source { get; set; }
        public char OutputCsvDelimiter { get; set; }
        public IDictionary<string, Csv.Transformation.Row.TransformerSettings[]> TransformationProfiles { get; set; }
        
        public class SourceSettings
        {
            public CsvFileSettings<StatementEntryColumnNames> StatementFile1 { get; set; }
            public CsvFileSettings<StatementEntryColumnNames> StatementFile2 { get; set; }

            public IDictionary<string, string[]> MerchantNameMap { get; set; }
        }

        public class CsvFileSettings<TColumnNameMap>
            where TColumnNameMap : class
        {
            public TColumnNameMap ColumnNames { get; set; }
            public char Delimiter { get; set; }
            public string DateFormat { get; set; }
            public string TransformationProfileName { get; set; }
        }
    }
}