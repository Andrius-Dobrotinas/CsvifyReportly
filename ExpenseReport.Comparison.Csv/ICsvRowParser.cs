using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public interface ICsvRowParser<T>
    {
        T Parse(string[] csvRow);
    }
}