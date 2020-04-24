using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Statement
{
    public interface ICsvRowParser<TItem>
    {
        TItem Parse(string[] csvRow);
    }
}