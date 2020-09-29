using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class MerchantNameComparerTests
    {
        private MerchantNameComparer target;
        private Mock<Bank.IMerchanNameVariationComparer> nameMapComparer;
        private Mock<IDetailsComparer> detailsComparer;

        [SetUp]
        public void Setup()
        {
            nameMapComparer = new Mock<Bank.IMerchanNameVariationComparer>();
            detailsComparer = new Mock<IDetailsComparer>();
            target = new MerchantNameComparer(nameMapComparer.Object, detailsComparer.Object);
        }

        [TestCase("merchant name", "statement details")]
        [TestCase("merchant 2", "statement 2")]
        public void Must_CheckWith_NameMapComparer(
            string merchantIdentifier,
            string statementDetails)
        {
            var result = target.AreEqual(statementDetails, merchantIdentifier);

            nameMapComparer.Verify(
                x => x.IsMatch(
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once,
                "Must invoke the method");

            nameMapComparer.Verify(
                x => x.IsMatch(
                    It.Is<string>(
                        arg => arg == merchantIdentifier),
                    It.IsAny<string>()),
                "Must pass Merchant info on to the method as a 1st param");

            nameMapComparer.Verify(
                x => x.IsMatch(
                    It.Is<string>(
                        arg => arg == merchantIdentifier),
                    It.Is<string>(
                        arg => arg == statementDetails)),
                "Must pass Statement Details on to the method as a 2nd param");
        }

        [Test]
        public void When_MercantNameIs_ReferencedByTheTransaction_Must_Return_True()
        {
            Setup_NameMapComparer(true);

            var result = target.AreEqual("statementDetails", "merchantIdentifier");

            Assert.IsTrue(result);
        }

        [Test]
        public void When_MercantNameIs_Not_ReferencedByTheTransaction_Must_CompareValues()
        {
            Setup_NameMapComparer(false);

            var statementDetails = "asd";
            var merchantIdentifier = "ok";

            var result = target.AreEqual(statementDetails, merchantIdentifier);

            detailsComparer.Verify(
                x => x.AreEqual(
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once,
                "Must invoke the method once");

            detailsComparer.Verify(
                x => x.AreEqual(
                    It.Is<string>(
                        arg => arg == statementDetails),
                    It.Is<string>(
                        arg => arg == merchantIdentifier)),
                "Must pass the details and merchant name onto the method");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void When_MercantNameIs_Not_ReferencedByTheTransaction_Must_Return_WhateverValueComparerReturns(bool valueComparerReturnValue)
        {
            Setup_NameMapComparer(false);            

            detailsComparer
                .Setup(x => x.AreEqual(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(valueComparerReturnValue);

            var result = target.AreEqual("statementDetails", "merchantIdentifier");

            Assert.AreEqual(valueComparerReturnValue, result);
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