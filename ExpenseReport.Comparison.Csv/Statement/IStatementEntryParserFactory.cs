using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement
{
    public interface IStatementEntryParserFactory<TStatementEntry>
    {
        ICsvRowParser<TStatementEntry> Build(IDictionary<string, int> columnIndexes);
    }
}