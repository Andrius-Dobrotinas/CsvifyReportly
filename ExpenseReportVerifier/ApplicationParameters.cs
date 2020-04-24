using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier
{
    public class ApplicationParameters<TColumnIndexMap1, TColumnIndexMap2>
        : ApplicationParameters1<TColumnIndexMap1, TColumnIndexMap2>
    {
        public IDictionary<string, string[]> MerchantNameMap { get; set; }
    }

    public class ApplicationParameters1<TColumnIndexMap1, TColumnIndexMap2>
    {
        public CsvFileParameters<TColumnIndexMap1> StatementCsvFile { get; set; }
        public CsvFileParameters<TColumnIndexMap2> TransactionsCsvFile { get; set; }
        public char OutputCsvDelimiter { get; set; }
    }

    public class CsvFileParameters<TColumnIndexMap>
    {
        public TColumnIndexMap ColumnIndexes { get; set; }
        public char Delimiter { get; set; }
    }

    public class StatementCsvFileParameters : CsvFileParameters<StatementEntryColumnIndexes>
    {
    }

    public class TransactionCsvFileParameters : CsvFileParameters<TransactionDetailsColumnIndexes>
    {
    }
}