using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.ExpenseReport
{
    public class StatementEntryWithSourceData : StatementEntry
    {
        public string[] SourceData { get; set; }
    }
}