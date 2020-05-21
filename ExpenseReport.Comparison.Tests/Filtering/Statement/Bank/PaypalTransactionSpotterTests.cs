using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Filtering.Statement.Bank
{
    class PaypalTransactionSpotterTests
    {
        PaypalTransactionSpotter target = new PaypalTransactionSpotter();

        [TestCase("PAYPAL *")]
        [TestCase("PAYPAL *JUNORECORDS EB")]
        [TestCase("PAYPAL *WHATEVER")]
        [TestCase("PayPal *WHATEVER")]
        [TestCase("Paypal *WHATEVER")]
        [TestCase("paypal *WHATEVER")]
        public void Must_Return_True__When_InputStartsWithASpecialString_CaseInsensitive(string detailsString)
        {
            var result = target.IsPaypalTransaction(detailsString);

            Assert.True(result);
        }

        [TestCase("PAYPAL")]
        [TestCase("PAYPAL WHATEVER")]
        [TestCase("PAYPAL* WHATEVER")]
        [TestCase("PAYPAL*WHATEVER")]
        public void Must_Return_False__When_InputStartsWithWord_PayPal_ButDoesNotMatchTheSpecialString(string detailsString)
        {
            var result = target.IsPaypalTransaction(detailsString);

            Assert.IsFalse(result);
        }

        [TestCase("")]
        [TestCase("APAYPAL WHATEVER")]
        [TestCase(" PAYPAL *WHATEVER")]
        [TestCase("WHATEVER")]
        [TestCase("WHATEVER PAYPAL")]
        [TestCase("WHATEVER PAYPAL *")]
        public void Must_Return_False__ForAllOtherSortsOfInput(string detailsString)
        {
            var result = target.IsPaypalTransaction(detailsString);

            Assert.IsFalse(result);
        }
    }
}