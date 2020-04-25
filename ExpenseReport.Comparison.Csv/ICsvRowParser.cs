using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public interface ICsvRowParser<TItem>
    {
        TItem Parse(string[] csvRow);
    }
}