using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement
{
    public class StatementEntryParser : ICsvRowParser<StatementEntryWithSourceData>
    {
        private readonly StatementEntryColumnIndexes columnMapping;
        private readonly string dateFormat;

        public StatementEntryParser(StatementEntryColumnIndexes columnMapping, string dateFormat)
        {
            if (string.IsNullOrEmpty(dateFormat)) throw new ArgumentNullException(nameof(dateFormat));

            this.columnMapping = columnMapping ?? throw new ArgumentNullException(nameof(columnMapping));
            this.dateFormat = dateFormat;
        }

        public StatementEntryWithSourceData Parse(string[] csvRow)
        {
            if (csvRow == null) throw new ArgumentNullException(nameof(csvRow));

            // TODO: error/null value handling

            return new StatementEntryWithSourceData
            {
                Date = CsvValueParser.ParseDateOrThrow(csvRow[columnMapping.Date], dateFormat).Date,
                Amount = CsvValueParser.ParseDecimalOrThrow(csvRow[columnMapping.Amount]),
                Details = csvRow[columnMapping.Details],
                SourceData = csvRow
            };
        }
    }
}
