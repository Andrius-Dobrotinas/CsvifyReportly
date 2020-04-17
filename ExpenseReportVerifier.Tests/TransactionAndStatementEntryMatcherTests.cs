using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport
{
    /* The are non-unit tests. Test cases here define different sets of input with their complete expected sets of output.
     * Separate test methods test separate scenarios and separate aspects of the results
     * (ie match, non-matched transcations and non-matched statement entries are all tested by separate methods)
     * This is so that test results are easier to follow and for the easy/er identification of failures.
     * 
     * Test methods have long signatures with unused parameters specifically so that all of them can work on the same test case data.
     */

    public class TransactionAndStatementEntryMatcherTests
    {
        TransactionAndStatementEntryMatcher target;
        Mock<ITransactionAndStatementEntryComparer> comparer;

        [SetUp]
        public void Setup()
        {
            comparer = new Mock<ITransactionAndStatementEntryComparer>();
            target = new TransactionAndStatementEntryMatcher(comparer.Object);

            // make the comparinator only compare the amounts
            comparer.Setup(
                x => x.AreEqual(
                    It.IsAny<TransactionDetails>(),
                    It.IsAny<StatementEntry>()))
                .Returns<TransactionDetails, StatementEntry>(
                    (trans, stmnt) => trans.Amount == stmnt.Amount);
        }

        [TestCaseSource(nameof(GetStatementAndTransactions))]
        public void Should_FindMatchingItems(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, result.Matches);
        }

        [TestCaseSource(nameof(GetStatementAndTransactions))]
        public void Should_FindNonMatching_StatementEntries(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyNonMatchingStatements(unmatchedStatementEntries, result.UnmatchedStatementEntries);
        }

        [TestCaseSource(nameof(Get_NoMatchingItems))]
        public void Should_FindNonMatching_Transactions(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyNonMatchinTransactions(unmatchedTransactions, result.UnmatchedTransactions);
        }

        [TestCaseSource(nameof(Get_NoMatchingItems))]
        public void When_TherAreNoMatchingItems_ReturnAnEmptyMatchResult(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            Assert.False(result.Matches.Any());
        }

        [TestCaseSource(nameof(Get_NoMatchingItems))]
        public void When_TherAreNoMatchingItems_ReturnAll_StatementEntries_ItemsAsUnmatched(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyNonMatchingStatements(unmatchedStatementEntries, result.UnmatchedStatementEntries);
        }

        [TestCaseSource(nameof(Get_NoMatchingItems))]
        public void When_TherAreNoMatchingItems_ReturnAll_Transactions_AsUnmatched(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyNonMatchinTransactions(unmatchedTransactions, result.UnmatchedTransactions);
        }

        [TestCaseSource(nameof(Get_SomeTransactionsDontHaveMatches))]
        public void When_SomeOfTheTransactionsDontHaveMatchingStatements__Should_FindMatchingItems(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, result.Matches);
        }

        [TestCaseSource(nameof(Get_SomeTransactionsDontHaveMatches))]
        public void When_SomeOfTheTransactionsDontHaveMatchingStatements__Should_FindNonMatching_StatementEntries(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyNonMatchingStatements(unmatchedStatementEntries, result.UnmatchedStatementEntries);
        }

        [TestCaseSource(nameof(Get_SomeTransactionsDontHaveMatches))]
        public void When_SomeOfTheTransactionsDontHaveMatchingStatements__Should_FindNonMatching_Transactions(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyNonMatchinTransactions(unmatchedTransactions, result.UnmatchedTransactions);
        }

        [TestCaseSource(nameof(Get_SomeStatementsDontHaveMatches))]
        public void When_SomeOfTheStatementsDontHaveMatchingTransactions__Should_FindMatchingItems(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyMatches(expectedMatches, result.Matches);
        }

        [TestCaseSource(nameof(Get_SomeStatementsDontHaveMatches))]
        public void When_SomeOfTheStatementsDontHaveMatchingTransactions__Should_FindNonMatching_StatementEntries(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyNonMatchingStatements(unmatchedStatementEntries, result.UnmatchedStatementEntries);
        }

        [TestCaseSource(nameof(Get_SomeStatementsDontHaveMatches))]
        public void When_SomeOfTheStatementsDontHaveMatchingTransactions__Should_FindNonMatching_Transactions(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> unmatchedStatementEntries,
            IList<TransactionDetails> unmatchedTransactions)
        {
            var result = target.CheckForMatches(statementEntries, transactions);

            VerifyNonMatchinTransactions(unmatchedTransactions, result.UnmatchedTransactions);
        }

        static IEnumerable<TestCaseData> Get_SomeTransactionsDontHaveMatches()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3" };

            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { trans1, trans2},
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                },
                new StatementEntry[] { },
                new TransactionDetails[] { trans2 });

            // repeated, with different order
            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans1, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                },
                new StatementEntry[] { },
                new TransactionDetails[] { trans1 });

            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                },
                new StatementEntry[] { },
                new TransactionDetails[] { trans1, trans3 });
        }

        static IEnumerable<TestCaseData> Get_SomeStatementsDontHaveMatches()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3" };

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2 },
                new TransactionDetails[] { trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                },
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { });

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3 },
                new TransactionDetails[] { trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                },
                new StatementEntry[] { statement2, statement3 },
                new TransactionDetails[] { });

            // repeat with matches not in the beginning
            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3 },
                new TransactionDetails[] { trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                },
                new StatementEntry[] { statement1, statement3 },
                new TransactionDetails[] { });
                //.SetName("3:1 = 1 match + 2 non-matches. Different values");

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2, statement3 },
                new TransactionDetails[] { trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                },
                new StatementEntry[] { statement1, statement3 },
                new TransactionDetails[] { });
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
                new TransactionDetails[] { trans1, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>> { },
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans1, trans3 });

            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement4 },
                new TransactionDetails[] { trans1, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>> { },
                new StatementEntry[] { statement2, statement4 },
                new TransactionDetails[] { trans1, trans3 });

            // no transactions at all
            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { },
                new List<Tuple<StatementEntry, TransactionDetails>> { },
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { });

            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1 },
                new TransactionDetails[] { },
                new List<Tuple<StatementEntry, TransactionDetails>> { },
                new StatementEntry[] { statement2, statement1 },
                new TransactionDetails[] { });

            // no statements at all
            yield return new TestCaseData(
                new StatementEntry[] { },
                new TransactionDetails[] { trans1 },
                new List<Tuple<StatementEntry, TransactionDetails>> { },
                new StatementEntry[] { },
                new TransactionDetails[] { trans1 });

            yield return new TestCaseData(
                new StatementEntry[] { },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>> { },
                new StatementEntry[] { },
                new TransactionDetails[] { trans1, trans2, trans3 });

            // no data in either collection
            yield return new TestCaseData(
                new StatementEntry[] { },
                new TransactionDetails[] { },
                new List<Tuple<StatementEntry, TransactionDetails>> { },
                new StatementEntry[] { },
                new TransactionDetails[] { });
        }

        static IEnumerable<TestCaseData> GetStatementAndTransactions()
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
                },
                new StatementEntry[] { },
                new TransactionDetails[] { });

            // repeated with different values
            yield return new TestCaseData(
                new StatementEntry[] { statement2 },
                new TransactionDetails[] { trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2)
                },
                new StatementEntry[] { },
                new TransactionDetails[] { });

            // multiple transactions and statements
            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1 },
                new TransactionDetails[] { trans1, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                },
                new StatementEntry[] { },
                new TransactionDetails[] { });

            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1 },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                },
                new StatementEntry[] { },
                new TransactionDetails[] { trans3 });

            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { trans1, trans2, trans3 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                },
                new StatementEntry[] { },
                new TransactionDetails[] { });

            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { trans1, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1)
                },
                new StatementEntry[] { statement3 },
                new TransactionDetails[] { });

            yield return new TestCaseData(
                new StatementEntry[] { statement2, statement1, statement3 },
                new TransactionDetails[] { trans3, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement2, trans2),
                    new Tuple<StatementEntry, TransactionDetails>(statement3, trans3)
                },
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { });
        }

        // TODO: cover this. This will require writing modifying the implementation
        static IEnumerable<TestCaseData> Get_WithDuplicates()
        {
            var statement1 = new StatementEntry { Amount = 1, Details = "Statement 1" };
            var statement2 = new StatementEntry { Amount = 2, Details = "Statement 2" };
            var statement3 = new StatementEntry { Amount = 3, Details = "Statement 3" };

            var trans1 = new TransactionDetails { Amount = 1, Merchant = "Transaction 1" };
            var trans2 = new TransactionDetails { Amount = 2, Merchant = "Transaction 2" };
            var trans3 = new TransactionDetails { Amount = 3, Merchant = "Transaction 3" };

            var trans1Clone = new TransactionDetails { Amount = trans1.Amount, Merchant = trans1.Merchant };
            var trans2Clone = new TransactionDetails { Amount = trans2.Amount, Merchant = trans2.Merchant };

            yield return new TestCaseData(
                new StatementEntry[] { statement1 },
                new TransactionDetails[] { trans1, trans1Clone },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                },
                new StatementEntry[] { },
                new TransactionDetails[] { trans1Clone });

            yield return new TestCaseData(
                new StatementEntry[] { statement1, statement2 },
                new TransactionDetails[] { trans1, trans1Clone, trans2Clone, trans2 },
                new List<Tuple<StatementEntry, TransactionDetails>>
                {
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans1),
                    new Tuple<StatementEntry, TransactionDetails>(statement1, trans2Clone),
                },
                new StatementEntry[] { },
                new TransactionDetails[] { trans1Clone, trans2 });

            // todo: same for duplicate statement entries
            // todo: when there's a match for a duplicate statemetn entry, both transactions shoudbe consumed

        }

        private void VerifyMatches(
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<Tuple<StatementEntry, TransactionDetails>> actualMatches)
        {
            foreach (var expectedMatch in expectedMatches)
            {
                // Verify matches
                var match = actualMatches.FirstOrDefault(x => x.Item1 == expectedMatch.Item1);
                Assert.NotNull(match, $"Statement entry {expectedMatch.Item1.Details} should have a match");

                Assert.AreEqual(expectedMatch.Item2, match.Item2, $"Statement entry {expectedMatch.Item1.Details} should be a match with Transaction {expectedMatch.Item2.Merchant}");
            }

            Assert.AreEqual(expectedMatches.Count, actualMatches.Count, "The overall number of matches");
        }

        private void VerifyNonMatchingStatements(
            IList<StatementEntry> expectedItems,
            IList<StatementEntry> actualItems)
        {
            foreach (var statementEntry in expectedItems)
            {
                var contains = actualItems.Contains(statementEntry);

                Assert.IsTrue(contains, $"The collection of unmatched Statement entries must contain entry {statementEntry.Details}");
            }

            Assert.AreEqual(expectedItems.Count, actualItems.Count, "The number of unmatched Statement entries");
        }

        private void VerifyNonMatchinTransactions(
            IList<TransactionDetails> expectedItems,
            IList<TransactionDetails> actualItems)
        {
            foreach (var transaction in expectedItems)
            {
                var contains = actualItems.Contains(transaction);

                Assert.IsTrue(contains, $"The collection of unmatched Statement entries must contain entry {transaction.Merchant}");
            }

            Assert.AreEqual(expectedItems.Count, actualItems.Count, "The number of unmatched Statement entries");
        }
    }
}