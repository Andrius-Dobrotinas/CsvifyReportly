using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport
{
    public class MatchFinderTests
    {
        MatchFinder target;
        Mock<IItemComparer> comparer;

        [SetUp]
        public void Setup()
        {
            comparer = new Mock<IItemComparer>();
            target = new MatchFinder(comparer.Object);

            // make the comparinator only compare the amounts
            comparer.Setup(
                x => x.AreEqual(
                    It.IsAny<TransactionDetails>(),
                    It.IsAny<StatementEntry>()))
                .Returns<TransactionDetails, StatementEntry>(
                    (trans, stmnt) => trans.Amount == stmnt.Amount);
        }

        [TestCaseSource(nameof(GetMatchingStatementAndTransactions))]
        public void Must_ReturnMatchingStatementAndTransactionPairs(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }

        [TestCaseSource(nameof(Get_MatchesWhereSomeTransactionsDontHaveMatches))]
        public void Must_ReturnMatchingStatementAndTransactionPairs_Even_When_ThereAreTransactionsWithNoMatches(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }
        
        [TestCaseSource(nameof(Get_MatchesWhereSomeStatementsDontHaveMatches))]
        public void Must_ReturnMatchingStatementAndTransactionPairs_Even_When_ThereAreStatementsWithNoMatches(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }

        [TestCaseSource(nameof(Get_WithDuplicateTransactions))]
        public void When_ThereIsMoreThanOneMatchingTransactionForAStatementItem__Must_PickFirstItem(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, matches);
        }

        [TestCaseSource(nameof(Get_NoMatchingItems))]
        public void When_ThereAreNoMatchingItems_Must_ReturnAnEmptyCollection(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            Assert.False(matches.Any());
        }

        [TestCaseSource(nameof(Get_StatementsWithNoTransactionsAtAll))]
        public void When_ThereAreNoTransactionsAtAll_Must_ReturnAnEmptyCollection(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            Assert.False(matches.Any());
        }

        [TestCaseSource(nameof(Get_StatementsWithNoStatementsAtAll))]
        public void When_ThereAreNoStatementsAtAll_Must_ReturnAnEmptyCollection(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions)
        {
            var matches = target.GetMatches(statementEntries, transactions);

            Assert.False(matches.Any());
        }

        static IEnumerable<TestCaseData> Get_MatchesWhereSomeTransactionsDontHaveMatches()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3" };
            var trans4 = new TransactionDetails { Amount = 4, Merchant = "Transaction 4" };
            var trans5 = new TransactionDetails { Amount = 5, Merchant = "Transaction 5" };

            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { trans1, trans2},
                new List<Tuple<StatementEntry, TransactionDetails>>
                { 
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                });

            // repeated, with different order
            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans1, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                });

            // more transactions unmatched
            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                });

            // has more than one matching pair
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1 },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                });

            // more matching pairs and more unmatched transactions
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { trans1, trans2, trans5, trans3, trans4 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                });
        }

        static IEnumerable<TestCaseData> Get_MatchesWhereSomeStatementsDontHaveMatches()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };
            var statement4 = new StatementEntry { Amount = 4, Details = "Statement 4" };
            var statement5 = new StatementEntry { Amount = 5, Details = "Statement 5" };

            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3" };

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2 },
                new TransactionDetails[] { trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                });

            // more unmatched items
            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3 },
                new TransactionDetails[] { trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                });

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3 },
                new TransactionDetails[] { trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                });

            // has more than one matching pair
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { trans1, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                });

            // even more unmatched items
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3, statement5, statement4 },
                new TransactionDetails[] { trans3, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3),
                });
        }

        static IEnumerable<TestCaseData> Get_NoMatchingItems()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };
            var statement4 = new StatementEntry { Amount = 4, Details = "Statement 4" };

            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3" };

            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans1, trans3 });

            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement4 },
                new TransactionDetails[] { trans1, trans3 });

            // no data in either collection
            yield return new TestCaseData(
                new StatementEntry[] { },
                new TransactionDetails[] { });
        }

        static IEnumerable<TestCaseData> Get_StatementsWithNoTransactionsAtAll()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { });

            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { });
        }

        static IEnumerable<TestCaseData> Get_StatementsWithNoStatementsAtAll()
        {
            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3" };

            yield return new TestCaseData(
                new StatementEntry[] { },
                new TransactionDetails[] { trans1 });

            yield return new TestCaseData(
                new StatementEntry[] { },
                new TransactionDetails[] { trans1, trans2, trans3 });
        }

        static IEnumerable<TestCaseData> GetMatchingStatementAndTransactions()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3" };

            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                });

            // repeated with different values
            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                });

            // multiple transactions and statements
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1 },
                new TransactionDetails[] { trans1, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                });

            // has unmatched transactions !!!!!!!!!!!
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1 },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                });

            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                });

            // has unmatched statements!
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { trans1, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                });

            // has unmatched statements!
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { trans3, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                });
        }

        static IEnumerable<TestCaseData> GetMatchingStatementAndTransactions_WithUnmatchedItems()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3" };

            // has both unmatched statements and transactions
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { trans3, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                });
        }

        // TODO: cover this. This will require writing modifying the implementation
        static IEnumerable<TestCaseData> Get_WithDuplicateTransactions()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1", Date = new DateTime(2020, 01, 01) };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2", Date = new DateTime(2020, 01, 02) };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3", Date = new DateTime(2020, 01, 02) };

            var trans1Clone = new TransactionDetails { Amount = trans1.Amount, Merchant = $"{trans1.Merchant} Clone", Date = trans1.Date.AddDays(1) };
            var trans2Clone = new TransactionDetails { Amount = trans2.Amount, Merchant = $"{trans2.Merchant} Clone", Date = trans1.Date.AddDays(1) };

            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { trans1, trans1Clone },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                });

            // transactions presented in a different order
            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { trans1Clone, trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1Clone),
                });

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2 },
                new TransactionDetails[] { trans1Clone, trans1, trans2Clone, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1Clone),
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2Clone),
                });

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3 },
                new TransactionDetails[] { trans1Clone, trans1, trans2Clone, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1Clone),
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2Clone),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3),
                });
        }

        private void VerifyMatches(
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<Tuple<StatementEntry, TransactionDetails>> actualMatches)
        {
            foreach (var expectedMatch in expectedMatches)
            {
                // Verify matches
                var actualMatch = actualMatches.FirstOrDefault(x => x.Item1 == expectedMatch.Item1);
                Assert.NotNull(actualMatch, $"Statement entry {expectedMatch.Item1.Details} should have a match");

                Assert.AreEqual(expectedMatch.Item2, actualMatch.Item2, $"Statement entry {expectedMatch.Item1.Details} should be a match with Transaction {expectedMatch.Item2.Merchant}, not {actualMatch.Item2.Merchant}");
            }

            Assert.AreEqual(expectedMatches.Count, actualMatches.Count, "The overall number of matches");
        }
    }
}