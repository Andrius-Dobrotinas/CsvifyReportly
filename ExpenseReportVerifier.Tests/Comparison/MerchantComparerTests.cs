using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public class MerchantComparerTests
    {
        private MerchantComparer target = new MerchantComparer();

        [TestCase("something'", "something'")]
        [TestCase("nuthin'", "NUTHIN'", Description = "Must be case-insensitive")]
        public void Must_ReturnTrue_When_TransactionIsNotPayPal_AndBothHaveSameMerchantInfo(
            string reportMerchant,
            string statementDetails)
        {
            var result = target.DoStatementDetailsReferToMerchant(statementDetails, reportMerchant, false);

            Assert.IsTrue(result);
        }

        [TestCase("something'", "PAYPAL *something'")]
        [TestCase("nuthin'", "PAYPAL *NUTHIN'", Description = "Must be case-insensitive")]
        public void Must_ReturnTrue_When_TransactionIsPayPal_AndTheStatementDetailHaveTheSameMerchantInfoPrefixedWithPayPalPrefix(
            string reportMerchant,
            string statementDetails)
        {
            var result = target.DoStatementDetailsReferToMerchant(statementDetails, reportMerchant, true);

            Assert.IsTrue(result);
        }
    }
}