using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.ExpenseReport
{
    public class StatementEntryParser
    {
        public static StatementEntryWithSourceData Parse(string[] csvRow)
        {
            // TODO: error/null value handling
            // TODO: get column indexes from a settings file

            return new StatementEntryWithSourceData
            {
                Date = DateTime.Parse(csvRow[0]).Date,
                Amount = decimal.Parse(csvRow[1]),
                Details = csvRow[2],
                SourceData = csvRow
            };
        }
    }
}
