using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier
{
    public class ApplicationSettings
    {
        public StatementCsvFileSettings StatementCsvFile { get; set; }
        public TransactionCsvFileSettings TransactionsCsvFile { get; set; }
        public char OutputCsvDelimiter { get; set; }
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