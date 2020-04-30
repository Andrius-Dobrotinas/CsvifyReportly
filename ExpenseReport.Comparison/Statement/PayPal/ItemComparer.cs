using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.PayPal
{
    public class ItemComparer : IItemComparer<StatementEntry, StatementEntry>
    {
        public bool AreEqual(StatementEntry transaction, StatementEntry statementEntry)
        {
            return AreAmountsEqual(transaction, statementEntry)
                && AreOrderNumbersEqual(transaction, statementEntry);
        }

        private static bool AreAmountsEqual(StatementEntry item1, StatementEntry item2)
        {
            return item1.Amount == item2.Amount;
        }

        private bool AreOrderNumbersEqual(StatementEntry transcation, StatementEntry statement)
        {
            // TODO: make it case-insensitive
            return transcation.Details == statement.Details;
        }
    }
}