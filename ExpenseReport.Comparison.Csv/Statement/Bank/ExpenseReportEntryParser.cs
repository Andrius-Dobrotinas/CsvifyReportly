using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement.Bank
{
    public class ExpenseReportEntryParser : ICsvRowParser<ExpenseReportEntryWithSourceData>
    {
        private readonly ExpenseReportEntryColumnIndexes columnMapping;
        private readonly string dateFormat;

        public ExpenseReportEntryParser(ExpenseReportEntryColumnIndexes columnMapping, string dateFormat)
        {
            if (string.IsNullOrEmpty(dateFormat)) throw new ArgumentNullException(nameof(dateFormat));

            this.columnMapping = columnMapping ?? throw new ArgumentNullException(nameof(columnMapping));
            this.dateFormat = dateFormat;
        }

        public ExpenseReportEntryWithSourceData Parse(string[] csvRow)
        {
            if (csvRow == null) throw new ArgumentNullException(nameof(csvRow));

            // TODO: error/null value handling

            return new ExpenseReportEntryWithSourceData
            {
                Date = CsvValueParser.ParseDateOrThrow(csvRow[columnMapping.Date], dateFormat).Date,
                Amount = CsvValueParser.ParseDecimalOrThrow(csvRow[columnMapping.Amount]),
                IsPayPal = CsvValueParser.ParseBoolOrThrow(csvRow[columnMapping.IsPayPal]),
                Merchant = csvRow[columnMapping.Merchant],
                SourceData = csvRow
            };
        }
    }
}