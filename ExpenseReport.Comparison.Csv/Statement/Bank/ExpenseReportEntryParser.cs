using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement.Bank
{
    public class ExpenseReportEntryParser : ICsvRowParser<ExpenseReportEntryWithSourceData>
    {
        private readonly ExpenseReportEntryColumnIndexes columnMapping;

        public ExpenseReportEntryParser(ExpenseReportEntryColumnIndexes columnMapping)
        {
            this.columnMapping = columnMapping;
        }

        public ExpenseReportEntryWithSourceData Parse(string[] csvRow)
        {
            if (csvRow == null) throw new ArgumentNullException(nameof(csvRow));

            // TODO: error/null value handling

            return new ExpenseReportEntryWithSourceData
            {
                Date = DateTime.Parse(csvRow[columnMapping.Date]).Date,
                Amount = decimal.Parse(csvRow[columnMapping.Amount]),
                IsPayPal = bool.Parse(csvRow[columnMapping.IsPayPal]),
                Merchant = csvRow[columnMapping.Merchant],
                SourceData = csvRow
            };
        }
    }
}