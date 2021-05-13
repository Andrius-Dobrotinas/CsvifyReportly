using Andy.ExpenseReport.Comparison.Statement;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison
{
    public partial class CollectionComparerTests
    {
        // TODO: use made up types
        CollectionComparer<StatementEntry, TransactionDetails> target;
        Mock<IMatchFinder<StatementEntry, TransactionDetails>> primaryMatcher;
        List<IMatchFinder<StatementEntry, TransactionDetails>> matchers;

        [SetUp]
        public void Setup()
        {
            primaryMatcher = new Mock<IMatchFinder<StatementEntry, TransactionDetails>>();
            matchers = new List<IMatchFinder<StatementEntry, TransactionDetails>>() { primaryMatcher.Object };
            target = new CollectionComparer<StatementEntry, TransactionDetails>(matchers);
        }

        void Setup_PrimaryItemMatcher_ToReturn(IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            Setup_ItemMatcher_ToReturn(primaryMatcher, expectedMatches);
        }

        void Setup_ItemMatcher_ToReturn(
            Mock<IMatchFinder<StatementEntry, TransactionDetails>> matcher,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            matcher.Setup(
                x => x.GetMatches(
                    It.IsAny<IList<StatementEntry>>(),
                    It.IsAny<IList<TransactionDetails>>()))
                .Returns(expectedMatches);
        }

        [TestCaseSource(nameof(GetMatches))]
        public void Must_ReturnWhateverMatchesTheMatcherReturns(
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            Setup_PrimaryItemMatcher_ToReturn(expectedMatches);

            var statementEntries = new StatementEntry[] { new StatementEntry() };
            var transactions = new TransactionDetails[] { new TransactionDetails() };

            var matchGroups = target.Compare(statementEntries, transactions).MatchGroups;

            Assert.AreSame(expectedMatches, matchGroups.First());
        }

        [TestCaseSource(nameof(GetNonMatchingTransactions))]
        public void Must_ReturnUnmatchedTransactions_InASeparateCollection_InNoParticularOrder(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<TransactionDetails> expectedUnmatchedTransactions)
        {
            Setup_PrimaryItemMatcher_ToReturn(expectedMatches);

            Run__Must_ReturnUnmatchedTransactions_InASeparateCollection_InNoParticularOrder(
                statementEntries,
                transactions,
                expectedUnmatchedTransactions);
        }

        private void Run__Must_ReturnUnmatchedTransactions_InASeparateCollection_InNoParticularOrder(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<TransactionDetails> expectedUnmatchedTransactions)
        {
            var actualUnmatchedTransactions = target.Compare(statementEntries, transactions)
                .UnmatchedTransactions2;

            foreach (var transaction in expectedUnmatchedTransactions)
            {
                Assert.IsTrue(actualUnmatchedTransactions.Contains(transaction),
                    $"Must pick transaction {transaction.Details} as unmatched");
            }

            Assert.AreEqual(expectedUnmatchedTransactions.Count, actualUnmatchedTransactions.Count,
                "The number of selected items");
        }

        [TestCaseSource(nameof(GetNonMatchingStatementEntries))]
        public void Must_ReturnUnmatchedStatementEntries_InASeparateCollection_InNoParticularOrder(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> expectedUnmatchedStatementEntries)
        {
            Setup_PrimaryItemMatcher_ToReturn(expectedMatches);

            Run__Must_ReturnUnmatchedStatementEntries_InASeparateCollection_InNoParticularOrder(
                statementEntries,
                transactions,
                expectedUnmatchedStatementEntries);
        }

        private void Run__Must_ReturnUnmatchedStatementEntries_InASeparateCollection_InNoParticularOrder(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<StatementEntry> expectedUnmatchedStatementEntries)
        {
            var actualUnmatchedStatementEntries = target.Compare(statementEntries, transactions)
                .UnmatchedTransactions1;

            foreach (var statementEntry in expectedUnmatchedStatementEntries)
            {
                Assert.IsTrue(actualUnmatchedStatementEntries.Contains(statementEntry),
                    $"Must pick statement entry {statementEntry.Details} as unmatched");
            }

            Assert.AreEqual(expectedUnmatchedStatementEntries.Count, actualUnmatchedStatementEntries.Count,
                "The number of selected items");
        }

        [TestCaseSource(nameof(GetNoUnmatchedStatementEntries))]
        public void When_ThereAreNoUnmatchedStatementEntries__Must_ReturnAnEmptyCorrespondingCollection(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            Setup_PrimaryItemMatcher_ToReturn(expectedMatches);

            var actualUnmatchedStatementEntries = target.Compare(statementEntries, transactions)
                .UnmatchedTransactions1;

            Assert.IsFalse(actualUnmatchedStatementEntries.Any());
        }

        [TestCaseSource(nameof(GetNoUnmatchedTransactionEntries))]
        public void When_ThereAreNoUnmatchedTransactionEntries__Must_ReturnAnEmptyCorrespondingCollection(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            Setup_PrimaryItemMatcher_ToReturn(expectedMatches);

            var actualUnmatchedTransactionEntries = target.Compare(statementEntries, transactions)
                .UnmatchedTransactions2;

            Assert.IsFalse(actualUnmatchedTransactionEntries.Any());
        }

        static IEnumerable<TestCaseData> GetMatches()
        {
            // a simple case with one unmatched item
            yield return new TestCaseData(
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(
                        new StatementEntry(),
                        new TransactionDetails())
                });

            yield return new TestCaseData(
                new List<Tuple<StatementEntry, TransactionDetails>>{});
        }

        static IEnumerable<TestCaseData> GetNonMatchingTransactions()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Details = "Transaction 3" };
            var trans4 = new TransactionDetails { Amount = 4, Details = "Transaction 4" };

            // simple case with one unmatched item
            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { trans1, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                },
                new TransactionDetails[] { trans2 });

            // three unmatched items
            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans1, trans2, trans3, trans4 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                },
                new TransactionDetails[] { trans1, trans3, trans4 });

            // three unmatched items, items returned in a different order
            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans1, trans2, trans3, trans4 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                },
                new TransactionDetails[] { trans4, trans1, trans3 });

            // two matches, more unmatched items
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement3 },
                new TransactionDetails[] { trans1, trans2, trans3, trans4 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                },
                new TransactionDetails[] { trans2, trans4 });

            // two matches, more unmatched items, items returned in a different order
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement3 },
                new TransactionDetails[] { trans1, trans2, trans3, trans4 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                },
                new TransactionDetails[] { trans4, trans2 });
        }

        static IEnumerable<TestCaseData> GetNonMatchingStatementEntries()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };
            var statement4 = new StatementEntry { Amount = 4, Details = "Statement 4" };

            var trans1 = new TransactionDetails { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Details = "Transaction 3" };
            var trans4 = new TransactionDetails { Amount = 4, Details = "Transaction 4" };

            // simple case with one unmatched item
            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2 },
                new TransactionDetails[] { trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                },
                new StatementEntry[] { statement2 });

            // two unmatched items
            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3, statement4 },
                new TransactionDetails[] { trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                },
                new StatementEntry[] { statement1, statement3, statement4 });

            // three unmatched items, returned in a different order
            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3, statement4 },
                new TransactionDetails[] { trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                },
                new StatementEntry[] { statement3, statement1, statement4 });

            // two matched items, two unmatched items
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement3, statement1, statement4 },
                new TransactionDetails[] { trans1, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                },
                new StatementEntry[] { statement2, statement4 });

            // two matched items, two unmatched items, returned in a different order
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement3, statement1, statement4 },
                new TransactionDetails[] { trans1, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                },
                new StatementEntry[] { statement4, statement2 });
        }

        static IEnumerable<TestCaseData> GetNoUnmatchedStatementEntries()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Details = "Transaction 3" };

            // simple case with one unmatched item
            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                });

            // multiple matches
            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3 },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                });
        }

        static IEnumerable<TestCaseData> GetNoUnmatchedTransactionEntries()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Details = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Details = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Details = "Transaction 3" };

            // simple case with one unmatched item
            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                });

            // multiple matches
            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3 },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                });
        }

        public class TransactionDetails
        {
            public decimal Amount { get; set; }
            public string Details { get; set; }
        }
    }
}