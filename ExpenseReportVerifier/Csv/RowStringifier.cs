using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Csv
{
    public static class RowStringifier
    {
        public static string Stringifififiify(IEnumerable<string> row, char delimiter)
        {
            // TODO wrap values that have a delimiter in quotation marks
            return string.Join(delimiter, row);
        }
    }
}