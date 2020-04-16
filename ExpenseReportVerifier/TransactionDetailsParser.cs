using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.ExpenseReport
{
    public class TransactionDetailsParser
    {
        public static TransactionDetails Parse(string[] row)
        {
            // TODO: error/null value handling
            // take indexes from a settings file

            return new TransactionDetails
            {
                Date = DateTime.Parse(row[0]).Date,
                Amount = decimal.Parse(row[2]),
                IsPayPal = bool.Parse(row[3]),
                Merchant = row[5]
            };
        }
    }
}
