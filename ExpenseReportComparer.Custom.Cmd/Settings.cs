using Andy.ExpenseReport.Comparison.Csv.Statement;
using Andy.ExpenseReport.Comparison.Csv.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Custom.Cmd
{
    public class Settings
    {
        public ExpenseReportComparisonSettings Source { get; set; }
        public char OutputCsvDelimiter { get; set; }
        public IDictionary<string, Csv.Transformation.Row.TransformerSettings[]> TransformationProfiles { get; set; }

        public class ExpenseReportComparisonSettings
        {
            public CsvFileSettings<StatementEntryColumnNames> StatementFile { get; set; }
            public CsvFileSettings<ExpenseReportEntryColumnNames> ExpenseReportFile { get; set; }
            public IDictionary<string, string[]> MerchantNameMap { get; set; }
            public int DateTolerance { get; set; }
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