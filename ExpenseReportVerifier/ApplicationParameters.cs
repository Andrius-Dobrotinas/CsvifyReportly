using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier
{
    public class ApplicationParameters
    {
        public StatementCsvFileParameters StatementCsvFile { get; set; }
        public TransactionCsvFileParameters TransactionsCsvFile { get; set; }
        public char OutputCsvDelimiter { get; set; }
    }

    public class StatementCsvFileParameters
    {
        public StatementEntryColumnIndexes ColumnIndexes { get; set; }
        public char Delimiter { get; set; }
    }

    public class TransactionCsvFileParameters
    {
        public TransactionDetailsColumnIndexes ColumnIndexes { get; set; }
        public char Delimiter { get; set; }
    }
}