using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport
{
    public class TransactionAndStatementEntryMatcherTests
    {
        private TransactionAndStatementEntryMatcher target;
        private Mock<IMatchingTransactionFinder> matchingTransactionFinder;

        [SetUp]
        public void Setup()
        {
            matchingTransactionFinder = new Mock<IMatchingTransactionFinder>();
            target = new TransactionAndStatementEntryMatcher(matchingTransactionFinder.Object);
        }

        [TestCaseSource(nameof(GetStatementAndTransactions))]
        public void Must_LookForAMatchingTransaction_ForEachStatementEntry(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions)
        {
            Setup_MatchingTransactionFinder_ToReturnFirstItem_EachTime();

            target.CheckForMatches(statementEntries, transactions);

            matchingTransactionFinder.Verify(
                x => x.GetFirstMatchingTransaction(
                    It.IsAny<StatementEntry>(),
                    It.IsAny<IList<TransactionDetails>>()),
                Times.Exactly(statementEntries.Count),
                "Must invoke the function the number of times there are statement entries");

            for(int i = 0; i < statementEntries.Count; i++)
            {
                var targetEntry = statementEntries[i];

                matchingTransactionFinder.Verify(
                    x => x.GetFirstMatchingTransaction(
                        It.Is<StatementEntry>(
                            arg => arg == targetEntry),
                        It.IsAny<IList<TransactionDetails>>()),
                    Times.Once,
                    $"Must invoke the function for each entry... that means for index {i} too!");

                matchingTransactionFinder.Verify(
                    x => x.GetFirstMatchingTransaction(
                        It.Is<StatementEntry>(
                            arg => arg == targetEntry),
                        It.Is<IList<TransactionDetails>>(
                            arg => transactions.SequenceEqual(arg))),
                    Times.Once,
                    $"Must pass on the transactions to the function. Doesn't have to be the same instance of collection, though");
            }
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void Must_PickFirstMatchingTransaction_ForEachStatementEntry(
            IList<TransactionDetails> transactions,
            IList<StatementEntry> statementEntries,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            foreach(var expected in expectedMatches)
                Setup_MatchingTransactionFinder(expected.Item1, expected.Item2);

            var matches = target.CheckForMatches(statementEntries, transactions)
                .Matches;

            for (int i = 0; i < expectedMatches.Count; i++)
            {
                var expected = expectedMatches[i];
                var actual = matches.FirstOrDefault(x => x.Item1 == expected.Item1);

                Assert.NotNull(actual, $"Must find a match for statement entry index {i}");

                Assert.NotNull(actual.Item2, $"Must include a match for statement entry index {i}");
            }

            Assert.AreEqual(expectedMatches.Count, matches.Count);
        }

        [TestCaseSource(nameof(GetTestCases_ForUnmatchedStatementEntries))]
        public void When_NoMatchingTransactionIsPresentForAStatementEntry_Must_AddTheStatementEntryToThe_CollectionOfUnmatchedStatements(
            IList<TransactionDetails> transactions,
            IList<StatementEntry> statementEntries,
            IList<StatementEntry> unmatchedStatementEntries)
        {
            // make it return something each time except for items i want to have no matches
            Setup_MatchingTransactionFinder_ToReturnFirstItem_EachTime();

            foreach (var entry in unmatchedStatementEntries)
                Setup_MatchingTransactionFinder(entry, null);

            var results = target.CheckForMatches(statementEntries, transactions);

            var actualItems = results.UnmatchedStatementEntries;

            for (int i = 0; i < unmatchedStatementEntries.Count; i++)
            {
                var expected = unmatchedStatementEntries[i];

                var presentInTheMatchesCollection = results.Matches.Any(x => x.Item1 == expected);
                Assert.IsFalse(presentInTheMatchesCollection,
                    $"The item index {i} must not be added to the Matches collection");

                Assert.IsTrue(actualItems.Contains(expected),
                    $"Must add statement index {i} to the collection of unmatched entries");
            }

            Assert.AreEqual(unmatchedStatementEntries.Count, actualItems.Count, "The number of unmatched items");
        }

        [TestCaseSource(nameof(GetTestCases_ForUnmatchedTransactions))]
        public void WhenATransactionHasNoMatchingStatementEntry_Must_AddTheTransactionToThe_CollectionOfUnmatchedTransactions(
            IList<TransactionDetails> transactions,
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            // make it return something each time except for items i want to have no matches
            Setup_MatchingTransactionFinder_ToReturnFirstItem_EachTime(unmatchedTransactions);

            var results = target.CheckForMatches(statementEntries, transactions);

            var actualItems = results.UnmatchedTransactions;

            for (int i = 0; i < unmatchedTransactions.Count; i++)
            {
                var expected = unmatchedTransactions[i];

                var presentInTheMatchesCollection = results.Matches.Any(x => x.Item2 == expected);
                Assert.IsFalse(presentInTheMatchesCollection,
                    $"The item index {i} must NOT be added to the Matches collection");

                Assert.IsTrue(actualItems.Contains(expected),
                    $"Must add statement index {i} to the collection of unmatched entries");
            }

            Assert.AreEqual(unmatchedTransactions.Count, actualItems.Count, "The number of unmatched items");
        }

        private static IEnumerable<TestCaseData> GetStatementAndTransactions()
        {
            var entry1 = new TransactionDetails { Amount = 5, Merchant = "Somethin'" };
            var entry2 = new TransactionDetails { Amount = 2, Merchant = "Nuthin'" };
            var entry3 = new TransactionDetails { Amount = 7.11m, Merchant = "Se7en 11even" };

            // Amount values in statement entries are they look different, but they aren't used in the test for anything
            var statement1 = new StatementEntry { Amount = 1 };
            var statement2 = new StatementEntry { Amount = 2 };

            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { entry1, entry2 });

            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { entry2, entry1, entry3 });

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2 },
                new TransactionDetails[] { entry1 });

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2 },
                new TransactionDetails[] { entry2, entry1, entry3 });
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            var entry1 = new TransactionDetails { Amount = 5, Merchant = "Somethin'" };
            var entry2 = new TransactionDetails { Amount = 2, Merchant = "Nuthin'" };
            var entry3 = new TransactionDetails { Amount = 7.11m, Merchant = "Se7en 11even" };
            var statementEntry1 = new StatementEntry { Amount = 10 };

            yield return new TestCaseData(
                new TransactionDetails[] { entry2, entry1, entry3 },
                new StatementEntry[] { statementEntry1 },
                new Tuple<StatementEntry, TransactionDetails>[]
                {
                    new Tuple<StatementEntry, TransactionDetails>(statementEntry1, entry1)
                });

            var statementEntry2 = new StatementEntry { Amount = 20 };

            yield return new TestCaseData(
                new TransactionDetails[] { entry2, entry1, entry3 },
                new StatementEntry[] { statementEntry1, statementEntry2 },
                new Tuple<StatementEntry, TransactionDetails>[]
                {
                    new Tuple<StatementEntry, TransactionDetails>(statementEntry1, entry1),
                    new Tuple<StatementEntry, TransactionDetails>(statementEntry2, entry2)
                });
        }

        private static IEnumerable<TestCaseData> GetTestCases_ForUnmatchedStatementEntries()
        {
            var entry1 = new TransactionDetails { Amount = 5, Merchant = "Somethin'" };
            var entry2 = new TransactionDetails { Amount = 2, Merchant = "Nuthin'" };
            var entry3 = new TransactionDetails { Amount = 7.11m, Merchant = "Se7en 11even" };
            var statementEntry1 = new StatementEntry { Amount = 10 };

            // No matching transactions
            yield return new TestCaseData(
                new TransactionDetails[] { entry1 },
                new StatementEntry[] { statementEntry1 },
                new StatementEntry[] { statementEntry1 });

            yield return new TestCaseData(
                new TransactionDetails[] { entry2, entry1, entry3 },
                new StatementEntry[] { statementEntry1 },
                new StatementEntry[] { statementEntry1 });

            var statementEntry2 = new StatementEntry { Amount = 20 };

            yield return new TestCaseData(
                new TransactionDetails[] { entry2, entry1, entry3 },
                new StatementEntry[] { statementEntry2, statementEntry1 },
                new StatementEntry[] { statementEntry1 });

            yield return new TestCaseData(
                new TransactionDetails[] { entry2, entry1, entry3 },
                new StatementEntry[] { statementEntry2, statementEntry1 },
                new StatementEntry[] { statementEntry1, statementEntry2 });

            // No transactions at all
            yield return new TestCaseData(
                new TransactionDetails[] { },
                new StatementEntry[] { statementEntry1 },
                new StatementEntry[] { statementEntry1 });
        }

        private static IEnumerable<TestCaseData> GetTestCases_ForUnmatchedTransactions()
        {
            var entry1 = new TransactionDetails { Amount = 5, Merchant = "Somethin'" };
            var entry2 = new TransactionDetails { Amount = 2, Merchant = "Nuthin'" };
            var entry3 = new TransactionDetails { Amount = 7.11m, Merchant = "Se7en 11even" };
            var statementEntry1 = new StatementEntry { Amount = 10 };

            yield return new TestCaseData(
                new TransactionDetails[] { entry2, entry1 },
                new StatementEntry[] { statementEntry1 },
                new TransactionDetails[] { entry1 });

            yield return new TestCaseData(
                new TransactionDetails[] { entry2, entry1, entry3 },
                new StatementEntry[] { statementEntry1 },
                new TransactionDetails[] { entry1, entry2 });

            /* mocking out this scenario is too much work. would have to make sure that all transaction
             * from the first collection, except those in the second collection, are returned.
             */
            //yield return new TestCaseData(
            //    new TransactionDetails[] { entry2, entry1, entry3 },
            //    new StatementEntry[] { statementEntry2, statementEntry1 },
            //    new TransactionDetails[] { entry2 });

            // No statement entries at all
            yield return new TestCaseData(
                new TransactionDetails[] { entry2 },
                new StatementEntry[] { },
                new TransactionDetails[] { entry2 });

            yield return new TestCaseData(
                new TransactionDetails[] { entry2, entry1 },
                new StatementEntry[] { },
                new TransactionDetails[] { entry2, entry1 });
        }

        private void Setup_MatchingTransactionFinder_ToReturnFirstItem_EachTime()
        {
            matchingTransactionFinder.Setup(
                x => x.GetFirstMatchingTransaction(
                    It.IsAny<StatementEntry>(),
                    It.IsAny<IList<TransactionDetails>>()))
                .Returns<StatementEntry, IList<TransactionDetails>>(
                    (stmnt, transes) => transes.First());
        }

        private void Setup_MatchingTransactionFinder_ToReturnFirstItem_EachTime(IList<TransactionDetails> exceptions)
        {
            matchingTransactionFinder.Setup(
                x => x.GetFirstMatchingTransaction(
                    It.IsAny<StatementEntry>(),
                    It.IsAny<IList<TransactionDetails>>()))
                .Returns<StatementEntry, IList<TransactionDetails>>(
                    (stmnt, transes) =>
                    {
                        return transes.Except(exceptions).FirstOrDefault();
                        });
        }

        private void Setup_MatchingTransactionFinder(StatementEntry statement, TransactionDetails returnValue)
        {
            matchingTransactionFinder.Setup(
                x => x.GetFirstMatchingTransaction(
                    It.Is<StatementEntry>(
                        arg => arg == statement),
                    It.IsAny<IList<TransactionDetails>>()))
                .Returns(returnValue);
        }
    }
}