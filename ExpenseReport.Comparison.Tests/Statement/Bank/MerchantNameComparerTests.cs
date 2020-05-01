using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public class MerchantNameComparerTests
    {
        private MerchantNameComparer target;
        private Mock<IMerchanNameVariationComparer> nameMapComparer;

        [SetUp]
        public void Setup()
        {
            nameMapComparer = new Mock<IMerchanNameVariationComparer>();
            target = new MerchantNameComparer(nameMapComparer.Object);
        }

        [TestCase("something'", "PAYPAL *something'")]
        [TestCase("match", "PAYPAL *match")]
        [TestCase("nuthin'", "PAYPAL *NUTHIN'", Description = "Must be case-insensitive")]
        public void When_ReportEntryIsPaypal_AndStatementDetailHaveTheSameMerchantInfoAfterPaypalPrefix(
            string reportMerchant,
            string statementDetails)
        {
            var result = target.DoStatementDetailsReferToMerchant(statementDetails, reportMerchant, true);

            Assert.IsTrue(result);
        }

        [TestCase("match", "PAYPAL *match")]
        [TestCase("no match", "PAYPAL *match")]
        public void When_ReportEntryIsPaypal__Should_NotCheckTheNameMap(
            string name,
            string detailsString)
        {
            var result = target.DoStatementDetailsReferToMerchant(name, detailsString, true);

            nameMapComparer
                .Verify(
                    x => x.IsMatch(
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Never);
        }

        [Test]
        public void When_NameMatchIsFoundInTheNameMap__Should_Return_True()
        {
            Setup_NameMapComparer(true);

            var result = target.DoStatementDetailsReferToMerchant("somethin' else", "by the Kinks", false);

            Assert.IsTrue(result);
        }

        [TestCase("something'", "something'", ExpectedResult = true)]
        [TestCase("nuthin'", "NUTHIN'", ExpectedResult = true, Description = "Must be case-insensitive")]
        [TestCase("something'", "somethin' else", ExpectedResult = false)]
        public bool When_NoPaypal_AndNameMapDoesNotContainGivenName__Must_PerformCaseInsensitiveComparison(
            string reportMerchant,
            string statementDetails)
        {
            Setup_NameMapComparer(false);

            var result = target.DoStatementDetailsReferToMerchant(statementDetails, reportMerchant, false);

            return result;
        }

        private void Setup_NameMapComparer(bool returnValue)
        {
            nameMapComparer
                .Setup(
                    x => x.IsMatch(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(returnValue);
        }
    }
}