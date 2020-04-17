//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;

//namespace Andy.ExpenseReport
//{
//    public class MatchingTransactionDetailsEntryFinderTests
//    {
//        private MatchingTransactionFinder target;
//        private Mock<ITransactionAndStatementEntryComparer> merchantComparer;

//        [SetUp]
//        public void Setup()
//        {
//            merchantComparer = new Mock<ITransactionAndStatementEntryComparer>();
//            target = new MatchingTransactionFinder(merchantComparer.Object);
//        }

//        [Test]
//        public void Must_UseTheMerchantComparer_ForEachTransaction_AmountsMatchThatOfTheStatement()
//        {
//            var entry1 = new TransactionDetails { Merchant = "One", IsPayPal = true };
//            var entry2 = new TransactionDetails { Merchant = "Two", IsPayPal = false };
//            var entry3 = new TransactionDetails { Merchant = "Three", IsPayPal = true };

//            var transaction = new TransactionDetails[] { entry1, entry2, entry3 };

//            var statementEntry = new StatementEntry { Details = "Deets" };

//            target.GetFirstMatchingTransaction(statementEntry, transaction);

//            merchantComparer.Verify(
//                x => x.DoStatementDetailsReferToMerchant(
//                    It.IsAny<string>(),
//                    It.IsAny<string>(),
//                    It.IsAny<bool>()),
//                Times.Exactly(transaction.Length),
//                "Must invoke the function the number of times that there are report entries");

//            for (int i = 0; i < transaction.Length; i++)
//            {
//                var entry = transaction[i];

//                merchantComparer.Verify(
//                    x => x.DoStatementDetailsReferToMerchant(
//                        It.Is<string>(
//                            arg => arg == statementEntry.Details),
//                        It.Is<string>(
//                            arg => arg == entry.Merchant),
//                        It.Is<bool>(
//                            arg => arg == entry.IsPayPal)),
//                    Times.Once,
//                    "Must pass on the statement entry's Details field each time");

//                merchantComparer.Verify(
//                    x => x.DoStatementDetailsReferToMerchant(
//                        It.IsAny<string>(),
//                        It.Is<string>(
//                            arg => arg == entry.Merchant),
//                        It.IsAny<bool>()),
//                    Times.Once,
//                    "Must pass on the report entry's Merchant info");

//                merchantComparer.Verify(
//                    x => x.DoStatementDetailsReferToMerchant(
//                        It.IsAny<string>(),
//                        It.Is<string>(
//                            arg => arg == entry.Merchant),
//                        It.Is<bool>(
//                            arg => arg == entry.IsPayPal)),
//                    Times.Once,
//                    "Must pass on the report entry's PayPal flag");
//            }
//        }

//        [Test]
//        public void When_ThereAreNoMatchingTransactions_Must_ReturnNull()
//        {
//            merchantComparer.Setup(
//                x => x.DoStatementDetailsReferToMerchant(
//                    It.IsAny<string>(),
//                    It.IsAny<string>(),
//                    It.IsAny<bool>()))
//                .Returns(false);

//            var entry1 = new TransactionDetails { Amount = 5 };
//            var entry2 = new TransactionDetails { Amount = 1 };

//            var transactions = new TransactionDetails[] { entry1, entry2 };

//            var statementEntry = new StatementEntry { Amount = -5 };

//            var result = target.GetFirstMatchingTransaction(statementEntry, transactions);

//            Assert.AreEqual(null, result);
//        }

//        [TestCaseSource(nameof(GetTestCases))]
//        public void Must_Select_ATransactionWithSamePriceAndMatchingMerchantString(
//            IList<TransactionDetails> transactions,
//            TransactionDetails targetEntry)
//        {
//            merchantComparer.Setup(
//                x => x.DoStatementDetailsReferToMerchant(
//                    It.IsAny<string>(),
//                    It.Is<string>(
//                        arg => arg == targetEntry.Merchant),
//                    It.IsAny<bool>()))
//                .Returns(true);

//            var statementEntry = new StatementEntry { Amount = targetEntry.Amount };

//            var result = target.GetFirstMatchingTransaction(statementEntry, transactions);

//            Assert.AreEqual(targetEntry, result);
//        }

//        private static IEnumerable<TestCaseData> GetTestCases()
//        {
//            var entry1 = new TransactionDetails { Amount = 5, Merchant = "Somethin'" };
//            var entry2 = new TransactionDetails { Amount = 2, Merchant = "Nuthin'" };
//            var entry3 = new TransactionDetails { Amount = 7.11m, Merchant = "Se7en 11even" };

//            yield return new TestCaseData(
//                new TransactionDetails[] { entry2, entry1, entry3 },
//                entry1);

//            yield return new TestCaseData(
//                new TransactionDetails[] { entry2, entry1, entry3 },
//                entry2);

//            yield return new TestCaseData(
//                new TransactionDetails[] { entry2, entry1, entry3 },
//                entry3);

//            var price = 7.01m;
//            var entry4 = new TransactionDetails { Amount = price, Merchant = "Unique" };

//            // When some prices are the same
//            yield return new TestCaseData(
//                new TransactionDetails[]
//                {
//                    entry4,
//                    new TransactionDetails { Amount = price, Merchant = "Unique Too" },
//                    new TransactionDetails { Amount = price, Merchant = "Unique, Too" },
//                    new TransactionDetails { Amount = 1, Merchant = "Unique, Too 2" }
//                },
//                entry4);

//            // When some transactions have identical merchant info and price
//            var entry5 = new TransactionDetails { Amount = 8.11m, Merchant = "Guitar Center" };
//            yield return new TestCaseData(
//                new TransactionDetails[]
//                {
//                    entry5,
//                    new TransactionDetails { Amount = 8.11m, Merchant = "Guitar Center" },
//                    new TransactionDetails { Amount = 7, Merchant = "Unique, Too" },
//                },
//                entry5);
//        }
//    }
//}