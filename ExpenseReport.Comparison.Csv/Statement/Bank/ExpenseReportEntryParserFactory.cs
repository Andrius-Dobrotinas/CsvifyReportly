using Andy.Csv.Transformation.Row;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement.Bank
{
    public class ExpenseReportEntryParserFactory
        : IStatementEntryParserFactory<ExpenseReportEntryWithSourceData>
    {
        private readonly ExpenseReportEntryColumnNames columnMapping;
        private readonly string dateFormat;

        public ExpenseReportEntryParserFactory(ExpenseReportEntryColumnNames columnMapping, string dateFormat)
        {
            this.columnMapping = columnMapping ?? throw new ArgumentNullException(nameof(columnMapping));

            if (string.IsNullOrEmpty(dateFormat)) throw new ArgumentNullException(nameof(dateFormat));
            this.dateFormat = dateFormat;
        }

        public ICsvRowParser<ExpenseReportEntryWithSourceData> Build(IDictionary<string, int> columnIndexes)
        {
            if (columnIndexes == null) throw new ArgumentNullException(nameof(columnIndexes));

            int amountColumnIndex = Column.GetOrThrow(columnIndexes, columnMapping.Amount);
            int dateColumnIndex = Column.GetOrThrow(columnIndexes, columnMapping.Date);
            int merchantColumnIndex = Column.GetOrThrow(columnIndexes, columnMapping.Merchant);
            int isPPColumnIndex = Column.GetOrThrow(columnIndexes, columnMapping.IsPayPal);

            return new ExpenseReportEntryParser(
                new ExpenseReportEntryColumnIndexes
                {
                    Amount = amountColumnIndex,
                    Date = dateColumnIndex,
                    Merchant = merchantColumnIndex,
                    IsPayPal = isPPColumnIndex
                },
                dateFormat);
        }
    }
}