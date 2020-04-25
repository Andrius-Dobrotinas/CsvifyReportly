using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class Parameters<TColumnIndexMap1, TColumnIndexMap2>
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
}