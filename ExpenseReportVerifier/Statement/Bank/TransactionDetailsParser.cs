using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Statement.Bank
{
    public class TransactionDetailsParser : ICsvRowParser<TransactionDetailsWithSourceData>
    {
        private readonly TransactionDetailsColumnIndexes columnMapping;

        public TransactionDetailsParser(TransactionDetailsColumnIndexes columnMapping)
        {
            this.columnMapping = columnMapping;
        }

        public TransactionDetailsWithSourceData Parse(string[] csvRow)
        {
            if (csvRow == null) throw new ArgumentNullException(nameof(csvRow));

            // TODO: error/null value handling

            return new TransactionDetailsWithSourceData
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