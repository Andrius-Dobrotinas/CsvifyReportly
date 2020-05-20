using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public class ItemComparer : IItemComparer<ExpenseReportEntry, StatementEntry>
    {
        private readonly IMerchantNameComparer merchantNameComparer;
        private readonly IAmountComparer amountComparer;
        private readonly IDateComparer dateComparer;

        public ItemComparer(
            IMerchantNameComparer merchantNameComparer,
            IAmountComparer amountComparer,
            IDateComparer dateComparer)
        {
            this.merchantNameComparer = merchantNameComparer;
            this.amountComparer = amountComparer;
            this.dateComparer = dateComparer;
        }

        public bool AreEqual(ExpenseReportEntry transaction, StatementEntry statementEntry)
        {
            return AreAmountsEqual(transaction, statementEntry)
                && AreMerchantsEqual(transaction, statementEntry)
                && AreDatesEqual(transaction, statementEntry);
        }

        private bool AreAmountsEqual(ExpenseReportEntry transactionDetails, StatementEntry statement)
        {
            return amountComparer.AreEqual(transactionDetails.Amount, statement.Amount);
        }

        private bool AreMerchantsEqual(ExpenseReportEntry transcation, StatementEntry statement)
        {
            return merchantNameComparer.DoStatementDetailsReferToMerchant(statement.Details, transcation.Merchant, transcation.IsPayPal);
        }

        private bool AreDatesEqual(ExpenseReportEntry transactionDetails, StatementEntry statement)
        {
            return dateComparer.AreDatesEqual(transactionDetails.Date, statement.Date);
        }
    }
}