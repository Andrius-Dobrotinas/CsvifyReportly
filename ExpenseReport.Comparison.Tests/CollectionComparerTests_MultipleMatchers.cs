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
        [TestCaseSource(nameof(GetMatches))]
        public void MoreThanOneMatcher__Must_ReturnWhateverMatchesTheMatcherReturns(
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            Setup_PrimaryItemMatcher_ToReturn(new List<Tuple<StatementEntry, TransactionDetails>> { });

            var secondaryMatcher = new Mock<IMatchFinder<StatementEntry, TransactionDetails>>();
            Setup_ItemMatcher_ToReturn(secondaryMatcher, expectedMatches);

            matchers.Add(secondaryMatcher.Object);

            var statementEntries = new StatementEntry[] { new StatementEntry() };
            var transactions = new TransactionDetails[] { new TransactionDetails() };

            var matchGroups = target.Compare(statementEntries, transactions).MatchGroups;

            Assert.AreEqual(2, matchGroups.Count, "The must be two match groups");
            Assert.AreSame(expectedMatches, matchGroups.Skip(1).First());
        }

        [TestCaseSource(nameof(GetNonMatchingTransactions))]
        public void MoreThanOneMatcher__Must_ReturnUnmatchedTransactions_InASeparateCollection_InNoParticularOrder2(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<TransactionDetails> expectedUnmatchedTransactions)
        {
            Setup_PrimaryItemMatcher_ToReturn(new List<Tuple<StatementEntry, TransactionDetails>> { });
            CreateAndAddASecondaryMatcher(expectedMatches);

            Run__Must_ReturnUnmatchedTransactions_InASeparateCollection_InNoParticularOrder(
                statementEntries,
                transactions,
                expectedUnmatchedTransactions);
        }

        [TestCaseSource(nameof(GetNonMatchingStatementEntries))]
        public void MoreThanOneMatcher__MoreThanOneMatcher__Must_ReturnUnmatchedStatementEntries_InASeparateCollection_InNoParticularOrder(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches,
            IList<StatementEntry> expectedUnmatchedStatementEntries)
        {
            Setup_PrimaryItemMatcher_ToReturn(new List<Tuple<StatementEntry, TransactionDetails>> { });
            CreateAndAddASecondaryMatcher(expectedMatches);

            Run__Must_ReturnUnmatchedStatementEntries_InASeparateCollection_InNoParticularOrder(
                statementEntries,
                transactions,
                expectedUnmatchedStatementEntries);
        }

        [TestCaseSource(nameof(GetNoUnmatchedStatementEntries))]
        public void MoreThanOneMatcher__When_ThereAreNoUnmatchedStatementEntries__Must_ReturnAnEmptyCorrespondingCollection(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            Setup_PrimaryItemMatcher_ToReturn(new List<Tuple<StatementEntry, TransactionDetails>> { });
            CreateAndAddASecondaryMatcher(expectedMatches);

            var actualUnmatchedStatementEntries = target.Compare(statementEntries, transactions)
                .UnmatchedTransactions1;

            Assert.IsFalse(actualUnmatchedStatementEntries.Any());
        }

        [TestCaseSource(nameof(GetNoUnmatchedTransactionEntries))]
        public void MoreThanOneMatcher__When_ThereAreNoUnmatchedTransactionEntries__Must_ReturnAnEmptyCorrespondingCollection(
            IList<StatementEntry> statementEntries,
            IList<TransactionDetails> transactions,
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            Setup_PrimaryItemMatcher_ToReturn(new List<Tuple<StatementEntry, TransactionDetails>> { });
            CreateAndAddASecondaryMatcher(expectedMatches);

            var actualUnmatchedTransactionEntries = target.Compare(statementEntries, transactions)
                .UnmatchedTransactions2;

            Assert.IsFalse(actualUnmatchedTransactionEntries.Any());
        }

        private Mock<IMatchFinder<StatementEntry, TransactionDetails>> CreateAndAddASecondaryMatcher(
            IList<Tuple<StatementEntry, TransactionDetails>> expectedMatches)
        {
            var secondaryMatcher = new Mock<IMatchFinder<StatementEntry, TransactionDetails>>();
            Setup_ItemMatcher_ToReturn(secondaryMatcher, expectedMatches);

            matchers.Add(secondaryMatcher.Object);

            return secondaryMatcher;
        }
    }
}