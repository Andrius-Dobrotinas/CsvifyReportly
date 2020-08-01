using Andy.Csv.Transformation.Row;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv.Statement
{
    public class StatementEntryParserFactory
        : IStatementEntryParserFactory<StatementEntryWithSourceData>
    {
        private readonly StatementEntryColumnNames columnMapping;
        private readonly string dateFormat;

        public StatementEntryParserFactory(StatementEntryColumnNames columnMapping, string dateFormat)
        {
            this.columnMapping = columnMapping ?? throw new ArgumentNullException(nameof(columnMapping));

            if (string.IsNullOrEmpty(dateFormat)) throw new ArgumentNullException(nameof(dateFormat));
            this.dateFormat = dateFormat;
        }

        public ICsvRowParser<StatementEntryWithSourceData> Build(IDictionary<string, int> columnIndexes)
        {
            if (columnIndexes == null) throw new ArgumentNullException(nameof(columnIndexes));

            int amountColumnIndex = Column.GetOrThrow(columnIndexes, columnMapping.Amount);
            int dateColumnIndex = Column.GetOrThrow(columnIndexes, columnMapping.Date);
            int detailsColumnIndex = Column.GetOrThrow(columnIndexes, columnMapping.Details);

            return new StatementEntryParser(
                new StatementEntryColumnIndexes
                {
                    Amount = amountColumnIndex,
                    Date = dateColumnIndex,
                    Details = detailsColumnIndex
                },
                dateFormat);
        }
    }
}