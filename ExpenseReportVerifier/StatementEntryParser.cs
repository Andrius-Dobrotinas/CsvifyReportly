using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier
{
    public class StatementEntryParser
    {
        private readonly StatementEntryColumnIndexes columnMapping;

        public StatementEntryParser(StatementEntryColumnIndexes columnMapping)
        {
            this.columnMapping = columnMapping;
        }

        public StatementEntryWithSourceData Parse(string[] csvRow)
        {
            if (csvRow == null) throw new ArgumentNullException(nameof(csvRow));

            // TODO: error/null value handling

            return new StatementEntryWithSourceData
            {
                Date = DateTime.Parse(csvRow[columnMapping.Date]).Date,
                Amount = decimal.Parse(csvRow[columnMapping.Amount]),
                Details = csvRow[columnMapping.Details],
                SourceData = csvRow
            };
        }
    }
}
