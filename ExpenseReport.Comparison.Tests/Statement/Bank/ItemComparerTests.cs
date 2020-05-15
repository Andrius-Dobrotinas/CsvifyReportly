using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    class ItemComparerTests
    {
        ItemComparer target;
        Mock<IMerchantNameComparer> merchantComparer;
        Mock<IDateComparer> dateComparer;

        [SetUp]
        public void Setup()
        {
            merchantComparer = new Mock<IMerchantNameComparer>();
            dateComparer = new Mock<IDateComparer>();
            target = new ItemComparer(merchantComparer.Object, dateComparer.Object);
            Setup_DateComparer(true);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void When_AmountsMatch__Must_CompareMerchantInfo(bool isPayPal)
        {
            var reportEntry = new ExpenseReportEntry { Amount = 0, Merchant = "One", IsPayPal = isPayPal };
            var statementEntry = new StatementEntry { Amount = 0, Details = "Deets" };

            target.AreEqual(reportEntry, statementEntry);

            merchantComparer.Verify(
                x => x.DoStatementDetailsReferToMerchant(
                    It.Is<string>(
                        arg => arg == statementEntry.Details),
                    It.IsAny<string>(),
                    It.IsAny<bool>()),
                Times.Once,
                $"Must delegate the comparison to a certain component");

            merchantComparer.Verify(
                x => x.DoStatementDetailsReferToMerchant(
                    It.Is<string>(
                        arg => arg == statementEntry.Details),
                    It.IsAny<string>(),
                    It.IsAny<bool>()),
                $"Must pass the value of statement {nameof(StatementEntry.Details)} on to the function as the 1st param");

            merchantComparer.Verify(
                x => x.DoStatementDetailsReferToMerchant(
                    It.IsAny<string>(),
                    It.Is<string>(
                        arg => arg == reportEntry.Merchant),
                    It.IsAny<bool>()),
                $"Must pass the value of report entry's {nameof(ExpenseReportEntry.Merchant)} on to the function as the 2nd param");

            merchantComparer.Verify(
                x => x.DoStatementDetailsReferToMerchant(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<bool>(
                        arg => arg == reportEntry.IsPayPal)),
                $"Must pass the value of report entry's {nameof(ExpenseReportEntry.IsPayPal)} on to the function as the 2nd param");
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(711)]
        public void When_Must_Return_True__When_Amounts_MerchantDeets_And_Dates_Match(decimal amount)
        {
            var reportEntry = new ExpenseReportEntry { Amount = amount };
            var statementEntry = new StatementEntry { Amount = amount };

            Setup_MerchantComparer(true);
            Setup_DateComparer(true);

            var result = target.AreEqual(reportEntry, statementEntry);

            Assert.AreEqual(true, result);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(7, 11)]
        public void Must_Return_False__When_AmountsDontMatch_RegardlessOfMerchantInfo(decimal transAmt, decimal stmntAmt)
        {
            var reportEntry = new ExpenseReportEntry { Amount = transAmt };
            var statementEntry = new StatementEntry { Amount = stmntAmt };

            Setup_MerchantComparer(true);

            var result = target.AreEqual(reportEntry, statementEntry);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Must_Return_False__When_AmountsMatch_ButMerchantDeetsDont()
        {
            var reportEntry = new ExpenseReportEntry { Amount = 52 };
            var statementEntry = new StatementEntry { Amount = 52 };

            Setup_MerchantComparer(false);

            var result = target.AreEqual(reportEntry, statementEntry);

            Assert.AreEqual(false, result);
        }

        private void Setup_MerchantComparer(bool returnValue)
        {
            merchantComparer.Setup(
                x => x.DoStatementDetailsReferToMerchant(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .Returns(returnValue);
        }

        private void Setup_DateComparer(bool returnValue)
        {
            dateComparer
                .Setup(x => x.AreDatesEqual(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).
                Returns(returnValue);
        }
    }
}