using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class ItemComparer : IItemComparer<StatementEntry, StatementEntry>
    {
        private readonly IDetailsComparer detailsComparer;
        private readonly IAmountComparer amountComparer;
        private readonly IDateComparer dateComparer;

        public ItemComparer(
            IDetailsComparer detailsComparer,
            IAmountComparer amountComparer,
            IDateComparer dateComparer)
        {
            this.detailsComparer = detailsComparer;
            this.amountComparer = amountComparer;
            this.dateComparer = dateComparer;
        }

        public bool AreEqual(StatementEntry transaction1, StatementEntry transaction2)
        {
            return AreAmountsEqual(transaction1, transaction2)
                && AreDetailsEqual(transaction1, transaction2)
                && AreDatesEqual(transaction1, transaction2);
        }

        private bool AreAmountsEqual(StatementEntry transaction1, StatementEntry transaction2)
        {
            return amountComparer.AreEqual(transaction1.Amount, transaction2.Amount);
        }

        private bool AreDetailsEqual(StatementEntry transaction1, StatementEntry transaction2)
        {
            return detailsComparer.AreEqual(transaction1.Details, transaction2.Details);
        }

        private bool AreDatesEqual(StatementEntry transaction1, StatementEntry transaction2)
        {
            return dateComparer.AreDatesEqual(transaction1.Date, transaction2.Date);
        }
    }
}