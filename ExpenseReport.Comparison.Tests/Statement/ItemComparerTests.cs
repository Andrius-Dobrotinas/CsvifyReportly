using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    class ItemComparerTests
    {
        ItemComparer target;
        Mock<IDetailsComparer> detailsComparer;
        Mock<IAmountComparer> amountComparer;
        Mock<IDateComparer> dateComparer;

        [SetUp]
        public void Setup()
        {
            detailsComparer = new Mock<IDetailsComparer>();
            amountComparer = new Mock<IAmountComparer>();
            dateComparer = new Mock<IDateComparer>();
            target = new ItemComparer(
                detailsComparer.Object,
                amountComparer.Object,
                dateComparer.Object);
        }

        [Test]
        public void Must_Return_True__When_Amounts_Details_And_Dates_Match()
        {
            Setup_AmountComparer(true);
            Setup_DetailsComparer(true);
            Setup_DateComparer(true);

            var result = target.AreEqual(new StatementEntry(), new StatementEntry());

            Assert.True(result);
        }

        [TestCase(true, false, false)]
        [TestCase(true, true, false)]
        [TestCase(false, true, false)]
        [TestCase(false, true, true)]
        [TestCase(false, false, true)]
        [TestCase(true, false, true)]
        public void Must_Return_False__When_OneOfTheComparersReturnsNegative(bool isAmountEqual, bool isMerchantEqual, bool isDateEqual)
        {
            Setup_AmountComparer(isAmountEqual);
            Setup_DetailsComparer(isMerchantEqual);
            Setup_DateComparer(isDateEqual);

            var result = target.AreEqual(new StatementEntry(), new StatementEntry());

            Assert.IsFalse(result);
        }        

        private void Setup_DetailsComparer(bool returnValue)
        {
            detailsComparer.Setup(
                x => x.AreEqual(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
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

        private void Setup_AmountComparer(bool returnValue)
        {
            amountComparer
                .Setup(x => x.AreEqual(
                    It.IsAny<decimal>(),
                    It.IsAny<decimal>())).
                Returns(returnValue);
        }
    }
}