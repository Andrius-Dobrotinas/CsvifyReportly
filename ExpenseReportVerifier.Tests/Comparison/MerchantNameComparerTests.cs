using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public class MerchantNameComparerTests
    {
        private MerchantNameComparer target = new MerchantNameComparer();

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

        [TestCase("Amazon", "Amazon")]
        [TestCase("Amazon", "AMAZON", Description = "Must be case-insensitive")]
        [TestCase("amazon", "Amazon", Description = "Must be case-insensitive")]
        [TestCase("Amazon", "AMAZON.CO.UK")]
        [TestCase("Amazon", "AMAZON.CO.UK*Y31OL8PS6")]
        [TestCase("Amazon", "AMAZON PRIME")]
        [TestCase("Amazon", "AMZNMKTPLACE")]
        [TestCase("Amazon", "AMZ*Amazon.co.uk")]
        public void Must_ReturnTrue_When_TransactionIsAmazon_AndTheStatementDetailHaveOneOfThePredefinedAmazonPrefixes_AndItsNotAPayPalTransaction(
            string reportMerchant,
            string statementDetails)
        {
            var result = target.DoStatementDetailsReferToMerchant(statementDetails, reportMerchant, false);

            Assert.IsTrue(result);
        }

        [Test]
        public void Must_ReturnFalse_When_TransactionIsAmazon_AndTheStatementDetailHaveOneOfThePredefinedAmazonPrefixes_ButItsAPayPalTransaction()
        {
            var result = target.DoStatementDetailsReferToMerchant("Amazon", "Amazon", true);

            Assert.IsFalse(result);
        }
    }
}