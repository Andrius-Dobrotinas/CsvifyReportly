using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class ItemComparer : IItemComparer<StatementEntry, StatementEntry>
    {
        public bool AreEqual(StatementEntry transaction, StatementEntry statementEntry)
        {
            return AreAmountsEqual(transaction, statementEntry)
                && AreOrderNumbersEqual(transaction, statementEntry);
        }

        private static bool AreAmountsEqual(StatementEntry entry1, StatementEntry entry2)
        {
            return entry1.Amount == entry2.Amount;
        }

        private bool AreOrderNumbersEqual(StatementEntry transcation, StatementEntry statement)
        {
            // TODO: make it case-insensitive
            return transcation.Details == statement.Details;
        }
    }
}