using Andy.ExpenseReport.Comparison.Csv.Statement;
using Andy.ExpenseReport.Comparison.Csv.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class Settings
    {
        public CsvFileSettings<StatementEntryColumnIndexes> StatementFile { get; set; }
        public CsvFileSettings<TransactionDetailsColumnIndexes> TransactionsFile { get; set; }
        public char OutputCsvDelimiter { get; set; }
        public IDictionary<string, string[]> MerchantNameMap { get; set; }
    }

    public class CsvFileSettings<TColumnIndexMap>
        where TColumnIndexMap : class
    {
        public TColumnIndexMap ColumnIndexes { get; set; }
        public char Delimiter { get; set; }
    }
}