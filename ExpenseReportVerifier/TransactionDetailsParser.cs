using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.ExpenseReport
{
    public class TransactionDetailsParser
    {
        public static TransactionDetailsWithSourceData Parse(string[] csvRow)
        {
            // TODO: error/null value handling
            // take indexes from a settings file

            return new TransactionDetailsWithSourceData
            {
                Date = DateTime.Parse(csvRow[0]).Date,
                Amount = decimal.Parse(csvRow[2]),
                IsPayPal = bool.Parse(csvRow[3]),
                Merchant = csvRow[5],
                SourceData = csvRow
            };
        }
    }
}