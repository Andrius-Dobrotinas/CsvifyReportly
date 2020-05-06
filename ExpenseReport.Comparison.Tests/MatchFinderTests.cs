using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison
{
    public class MatchFinderTests
    {
        MatchFinder<TransactionType1, TransactionType2> target;
        Mock<IItemComparer<TransactionType2, TransactionType1>> comparer;

        [SetUp]
        public void Setup()
        {
            comparer = new Mock<IItemComparer<TransactionType2, TransactionType1>>();
            target = new MatchFinder<TransactionType1, TransactionType2>(comparer.Object);

            // make the comparinator only compare the amounts
            comparer.Setup(
                x => x.AreEqual(
                    It.IsAny<TransactionType2>(),
                    It.IsAny<TransactionType1>()))
                .Returns<TransactionType2, TransactionType1>(
                    (trans, stmnt) => trans.Amount == stmnt.Amount);
        }

        [TestCaseSource(nameof(GetMatchingStatementAndTransactions))]
        public void Must_ReturnMatchingStatementAndTransactionPairs(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions,
            IList<Tuple<TransactionType1, TransactionType2>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }

        [TestCaseSource(nameof(Get_MatchesWhereSomeTransactionsDontHaveMatches))]
        public void Must_ReturnMatchingStatementAndTransactionPairs_Even_When_ThereAreTransactionsWithNoMatches(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions,
            IList<Tuple<TransactionType1, TransactionType2>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }
        
        [TestCaseSource(nameof(Get_MatchesWhereSomeStatementsDontHaveMatches))]
        public void Must_ReturnMatchingStatementAndTransactionPairs_Even_When_ThereAreStatementsWithNoMatches(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions,
            IList<Tuple<TransactionType1, TransactionType2>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }

        [TestCaseSource(nameof(Get_WithDuplicateTransactions))]
        public void When_ThereIsMoreThanOneMatchingTransactionForAStatementItem__Must_PickFirstItem(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions,
            IList<Tuple<TransactionType1, TransactionType2>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }

        [TestCaseSource(nameof(Get_WithDuplicateStatementEntries))]
        public void When_ThereIsMoreThanOneIdenticalStatementEntry_ButThereAreLessMatchingTransactions__Must_PickFirstItem(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions,
            IList<Tuple<TransactionType1, TransactionType2>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }

        [TestCaseSource(nameof(Get_WithEqualNumberOfDuplicateStatementAndTransactionEntries))]
        public void When_ThereIsMoreThanOneIdenticalStatementEntry_AndThereIsTheSameNumberOfMatchingTransactions_PickThemAll(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions,
            IList<Tuple<TransactionType1, TransactionType2>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }

        [TestCaseSource(nameof(Get_DuplicateStatementEntriesWithMoreTransactions))]
        public void When_ThereIsMoreThanOneIdenticalStatementEntry_AndThereAreEvenMoreMatchingTransactions_PickThemFirstMatchingTransactions(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions,
            IList<Tuple<TransactionType1, TransactionType2>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }

        [TestCaseSource(nameof(Get_NoMatchingItems))]
        public void When_ThereAreNoMatchingItems_Must_ReturnAnEmptyCollection(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            Assert.False(matches.Any());
        }

        [TestCaseSource(nameof(Get_StatementsWithNoTransactionsAtAll))]
        public void When_ThereAreNoTransactionsAtAll_Must_ReturnAnEmptyCollection(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            Assert.False(matches.Any());
        }

        [TestCaseSource(nameof(Get_StatementsWithNoStatementsAtAll))]
        public void When_ThereAreNoStatementsAtAll_Must_ReturnAnEmptyCollection(
            IList<TransactionType1> statementEntries,
            IList<TransactionType2> transactions)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            Assert.False(matches.Any());
        }

        [Test]
        public void When_ThereAreMatches__Must_NotAlterTheOriginalCollection1()
        {
            var transactions1 = new TransactionType1[]
            {
                new TransactionType1 { Amount = 1 }, new TransactionType1 { Amount = 2 }
            };
            var transactions2 = new TransactionType2[]
            {
                new TransactionType2 { Amount = 1 }, new TransactionType2 { Amount = 2 }
            };

            var transactions1Copy = transactions1.ToArray();

            target.GetMatches(transactions1, transactions2);

            Assert.AreEqual(transactions1Copy.Length, transactions1.Length);
            Assert.IsTrue(transactions1Copy.SequenceEqual(transactions1));
        }

        [Test]
        public void When_ThereAreMatches__Must_NotAlterTheOriginalCollection2()
        {
            var transactions1 = new TransactionType1[]
            {
                new TransactionType1 { Amount = 1 }, new TransactionType1 { Amount = 2 }
            };
            var transactions2 = new TransactionType2[]
            { 
                new TransactionType2 { Amount = 1 }, new TransactionType2 { Amount = 2 }
            };

            var transactions2Copy = transactions2.ToArray();

            target.GetMatches(transactions1, transactions2);

            Assert.AreEqual(transactions2Copy.Length, transactions2.Length);
            Assert.IsTrue(transactions2Copy.SequenceEqual(transactions2));
        }

        static IEnumerable<TestCaseData> Get_MatchesWhereSomeTransactionsDontHaveMatches()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3" };
            var trans4 = new TransactionType2 { Amount = 4, Details = "Transaction 4" };
            var trans5 = new TransactionType2 { Amount = 5, Details = "Transaction 5" };

            yield return new TestCaseData(
                new TransactionType1[] { statement1 },
                new TransactionType2[] { trans1, trans2},
                new List<Tuple<TransactionType1, TransactionType2>>
                { 
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1)
                });

            // repeated, with different order
            yield return new TestCaseData(
                new TransactionType1[] { statement2 },
                new TransactionType2[] { trans1, trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2)
                });

            // more transactions unmatched
            yield return new TestCaseData(
                new TransactionType1[] { statement2 },
                new TransactionType2[] { trans1, trans2, trans3 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2)
                });

            // has more than one matching pair
            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1 },
                new TransactionType2[] { trans1, trans2, trans3 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2)
                });

            // more matching pairs and more unmatched transactions
            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1, statement3 },
                new TransactionType2[] { trans1, trans2, trans5, trans3, trans4 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement3, trans3)
                });
        }

        static IEnumerable<TestCaseData> Get_MatchesWhereSomeStatementsDontHaveMatches()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };
            var statement4 = new TransactionType1 { Amount = 4, Details = "Statement 4" };
            var statement5 = new TransactionType1 { Amount = 5, Details = "Statement 5" };

            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3" };

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement2 },
                new TransactionType2[] { trans1 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1)
                });

            // more unmatched items
            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement2, statement3 },
                new TransactionType2[] { trans1 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1)
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement2, statement3 },
                new TransactionType2[] { trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2)
                });

            // has more than one matching pair
            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1, statement3 },
                new TransactionType2[] { trans1, trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2)
                });

            // even more unmatched items
            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1, statement3, statement5, statement4 },
                new TransactionType2[] { trans3, trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement3, trans3),
                });
        }

        static IEnumerable<TestCaseData> Get_NoMatchingItems()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };
            var statement4 = new TransactionType1 { Amount = 4, Details = "Statement 4" };

            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3" };

            yield return new TestCaseData(
                new TransactionType1[] { statement2 },
                new TransactionType2[] { trans1, trans3 });

            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement4 },
                new TransactionType2[] { trans1, trans3 });

            // no data in either collection
            yield return new TestCaseData(
                new TransactionType1[] { },
                new TransactionType2[] { });
        }

        static IEnumerable<TestCaseData> Get_StatementsWithNoTransactionsAtAll()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };

            yield return new TestCaseData(
                new TransactionType1[] { statement2 },
                new TransactionType2[] { });

            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1, statement3 },
                new TransactionType2[] { });
        }

        static IEnumerable<TestCaseData> Get_StatementsWithNoStatementsAtAll()
        {
            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3" };

            yield return new TestCaseData(
                new TransactionType1[] { },
                new TransactionType2[] { trans1 });

            yield return new TestCaseData(
                new TransactionType1[] { },
                new TransactionType2[] { trans1, trans2, trans3 });
        }

        static IEnumerable<TestCaseData> GetMatchingStatementAndTransactions()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3" };

            yield return new TestCaseData(
                new TransactionType1[] { statement1 },
                new TransactionType2[] { trans1 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1)
                });

            // repeated with different values
            yield return new TestCaseData(
                new TransactionType1[] { statement2 },
                new TransactionType2[] { trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2)
                });

            // multiple transactions and statements
            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1 },
                new TransactionType2[] { trans1, trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1)
                });

            // has unmatched transactions !!!!!!!!!!!
            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1 },
                new TransactionType2[] { trans1, trans2, trans3 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1)
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1, statement3 },
                new TransactionType2[] { trans1, trans2, trans3 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement3, trans3)
                });

            // has unmatched statements!
            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1, statement3 },
                new TransactionType2[] { trans1, trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1)
                });

            // has unmatched statements!
            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1, statement3 },
                new TransactionType2[] { trans3, trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement3, trans3)
                });
        }

        static IEnumerable<TestCaseData> GetMatchingStatementAndTransactions_WithUnmatchedItems()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3" };

            // has both unmatched statements and transactions
            yield return new TestCaseData(
                new TransactionType1[] { statement2, statement1, statement3 },
                new TransactionType2[] { trans3, trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement3, trans3)
                });
        }
        
        static IEnumerable<TestCaseData> Get_WithDuplicateTransactions()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3" };

            var trans1Clone = new TransactionType2 { Amount = trans1.Amount, Details = $"{trans1.Details} Clone" };
            var trans2Clone = new TransactionType2 { Amount = trans2.Amount, Details = $"{trans2.Details} Clone" };

            yield return new TestCaseData(
                new TransactionType1[] { statement1 },
                new TransactionType2[] { trans1, trans1Clone },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                });

            // transactions presented in a different order
            yield return new TestCaseData(
                new TransactionType1[] { statement1 },
                new TransactionType2[] { trans1Clone, trans1 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1Clone),
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement2 },
                new TransactionType2[] { trans1Clone, trans1, trans2Clone, trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1Clone),
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2Clone),
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement2, statement3 },
                new TransactionType2[] { trans1Clone, trans1, trans2Clone, trans2, trans3 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1Clone),
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2Clone),
                    new Tuple<TransactionType1, TransactionType2>(statement3, trans3),
                });
        }

        static IEnumerable<TestCaseData> Get_WithDuplicateStatementEntries()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };

            var statement1Clone = new TransactionType1 { Amount = statement1.Amount, Details = $"{statement1.Details} Clone" };

            var statement1Clone2 = new TransactionType1 { Amount = statement1.Amount, Details = $"{statement1.Details} Clone 2" };

            var statement2Clone = new TransactionType1 { Amount = statement2.Amount, Details = $"{statement2.Details} Clone" };

            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3" };

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement1Clone },
                new TransactionType2[] { trans1 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                });

            // statements presented in a different order
            yield return new TestCaseData(
                new TransactionType1[] { statement1Clone, statement1 },
                new TransactionType2[] { trans1 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1Clone, trans1),
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement1Clone, statement1, statement2, statement2Clone },
                new TransactionType2[] { trans1, trans2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1Clone, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement1Clone, statement2Clone, statement2, statement3 },
                new TransactionType2[] { trans1, trans2, trans3 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement2Clone, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement3, trans3),
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement1Clone, statement1Clone2 },
                new TransactionType2[] { trans1, trans2, trans3 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                });
        }

        static IEnumerable<TestCaseData> Get_WithEqualNumberOfDuplicateStatementAndTransactionEntries()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };

            var statement1Clone = new TransactionType1 { Amount = statement1.Amount, Details = $"{statement1.Details} Clone" };

            var statement1Clone2 = new TransactionType1 { Amount = statement1.Amount, Details = $"{statement1.Details} Clone 2" };

            var statement2Clone = new TransactionType1 { Amount = statement2.Amount, Details = $"{statement2.Details} Clone" };

            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3" };

            var trans1Clone = new TransactionType2 { Amount = trans1.Amount, Details = $"{trans1.Details} Clone" };
            var trans2Clone = new TransactionType2 { Amount = trans2.Amount, Details = $"{trans2.Details} Clone" };

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement1Clone },
                new TransactionType2[] { trans1, trans1Clone },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement1Clone, trans1Clone),
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement1Clone, statement1, statement2, statement2Clone },
                new TransactionType2[] { trans1, trans1Clone, trans2, trans2Clone },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1Clone, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1Clone),
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement2Clone, trans2Clone),
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement1Clone, statement1, statement3, statement2, statement2Clone },
                new TransactionType2[] { trans1, trans1Clone, trans2, trans2Clone },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1Clone, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1Clone),
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement2Clone, trans2Clone),
                });
        }

        static IEnumerable<TestCaseData> Get_DuplicateStatementEntriesWithMoreTransactions()
        {
            var statement1 = new TransactionType1 { Amount = 1, Details = "Statement 1" };
            var statement2 = new TransactionType1 { Amount = 2, Details = "Statement 2" };
            var statement3 = new TransactionType1 { Amount = 3, Details = "Statement 3" };

            var statement1Clone = new TransactionType1 { Amount = statement1.Amount, Details = $"{statement1.Details} Clone" };

            var statement1Clone2 = new TransactionType1 { Amount = statement1.Amount, Details = $"{statement1.Details} Clone 2" };

            var statement2Clone = new TransactionType1 { Amount = statement2.Amount, Details = $"{statement2.Details} Clone" };

            var trans1 = new TransactionType2 { Amount = 1, Details = "Transaction 1", };
            var trans2 = new TransactionType2 { Amount = 2, Details = "Transaction 2", };
            var trans3 = new TransactionType2 { Amount = 3, Details = "Transaction 3", };

            var trans1Clone = new TransactionType2 { Amount = trans1.Amount, Details = $"{trans1.Details} Clone" };

            var trans1Clone2 = new TransactionType2 { Amount = trans1.Amount, Details = $"{trans1.Details} Clone 2" };

            var trans2Clone = new TransactionType2 { Amount = trans2.Amount, Details = $"{trans2.Details} Clone" };

            var trans2Clone2 = new TransactionType2 { Amount = trans2.Amount, Details = $"{trans2.Details} Clone 2" };

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement1Clone },
                new TransactionType2[] { trans1, trans1Clone, trans1Clone2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement1Clone, trans1Clone),
                });

            yield return new TestCaseData(
                new TransactionType1[] { statement1, statement1Clone, statement2, statement2Clone },
                new TransactionType2[] { trans1, trans1Clone, trans1Clone2, trans2, trans2Clone, trans2Clone2 },
                new List<Tuple<TransactionType1, TransactionType2>>
                {
                    new Tuple<TransactionType1, TransactionType2>(statement1, trans1),
                    new Tuple<TransactionType1, TransactionType2>(statement1Clone, trans1Clone),
                    new Tuple<TransactionType1, TransactionType2>(statement2, trans2),
                    new Tuple<TransactionType1, TransactionType2>(statement2Clone, trans2Clone),
                });
        }

        private void VerifyMatches(
            IList<Tuple<TransactionType1, TransactionType2>> expectedMatches,
            IList<Tuple<TransactionType1, TransactionType2>> actualMatches)
        {
            foreach (var expectedMatch in expectedMatches)
            {
                // Verify matches
                var actualMatch = actualMatches.FirstOrDefault(x => x.Item1 == expectedMatch.Item1);
                Assert.NotNull(actualMatch, $"Statement entry {expectedMatch.Item1.Details} should have a match");

                Assert.AreEqual(expectedMatch.Item2, actualMatch.Item2, $"Statement entry {expectedMatch.Item1.Details} should be a match with Transaction {expectedMatch.Item2.Details}, not {actualMatch.Item2.Details}");
            }

            Assert.AreEqual(expectedMatches.Count, actualMatches.Count, "The overall number of matches");
        }

        public class TransactionType1
        {
            public decimal Amount { get; set; }
            public string Details { get; set; }
        }

        public class TransactionType2
        {
            public decimal Amount { get; set; }
            public string Details { get; set; }
        }
    }
}