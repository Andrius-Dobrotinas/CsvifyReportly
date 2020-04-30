using Andy.ExpenseReport.Comparison.Csv.Statement;
using Andy.ExpenseReport.Comparison.Csv.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class Settings
    {
        public StatementSourceSetting<StatementEntryColumnIndexes, TransactionDetailsColumnIndexes> Bank { get; set; }
        public StatementSourceSetting<StatementEntryColumnIndexes, StatementEntryColumnIndexes> PayPal { get; set; }
        public char OutputCsvDelimiter { get; set; }
        public IDictionary<string, string[]> MerchantNameMap { get; set; }
    }

    public class StatementSourceSetting<TStatementColumnIndexMap, TTransactionColumnIndexMap>
        where TStatementColumnIndexMap : class
        where TTransactionColumnIndexMap : class
    {
        public CsvFileSettings<TStatementColumnIndexMap> StatementFile { get; set; }
        public CsvFileSettings<TTransactionColumnIndexMap> TransactionsFile { get; set; }
    }

    public class CsvFileSettings<TColumnIndexMap>
        where TColumnIndexMap : class
    {
        public TColumnIndexMap ColumnIndexes { get; set; }
        public char Delimiter { get; set; }
    }
}