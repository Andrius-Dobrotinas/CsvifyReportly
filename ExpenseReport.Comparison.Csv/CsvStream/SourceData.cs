using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class SourceData
    {
        public IList<string[]> Transactions { get; set; }
        public IList<string[]> StatementEntries { get; set; }
        public int TransactionColumnCount { get; set; }
        public int StatementColumnCount { get; set; }
    }
}
