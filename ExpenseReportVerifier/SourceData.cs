using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Cmd
{
    public class SourceData
    {
        public IList<TransactionDetailsWithSourceData> Transactions { get; set; }
        public IList<StatementEntryWithSourceData> StatementEntries { get; set; }
        public int TransactionColumnCount { get; set; }
        public int StatementColumnCount { get; set; }
    }
}
