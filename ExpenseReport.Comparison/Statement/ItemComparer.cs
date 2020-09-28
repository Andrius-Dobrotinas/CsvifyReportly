using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class ItemComparer : IItemComparer<StatementEntry, StatementEntry>
    {
        public bool AreEqual(StatementEntry transaction1, StatementEntry transaction2)
        {
            return AreAmountsEqual(transaction1, transaction2)
                && AreDetailsEqual(transaction1, transaction2);
        }

        private static bool AreAmountsEqual(StatementEntry transaction1, StatementEntry transaction2)
        {
            return transaction1.Amount == transaction2.Amount;
        }

        private bool AreDetailsEqual(StatementEntry transaction1, StatementEntry transaction2)
        {
            // TODO: make it case-insensitive
            return transaction1.Details == transaction2.Details;
        }
    }
}