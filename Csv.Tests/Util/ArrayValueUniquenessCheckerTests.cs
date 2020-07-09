using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv
{
    public class ArrayValueUniquenessCheckerTests
    {
        ArrayValueUniquenessChecker target = new ArrayValueUniquenessChecker();

        [Test]
        public void When_InputIsEmpty__Must_Return_False()
        {
            var result = target.HasDuplicates(new string[0]);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_UniqueItems))]
        public void Must_Return_False_WhenAllItemsInTheCollectionAreUnique_CaseSensitive(
            IList<string> items)
        {
            var result = target.HasDuplicates(items);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_NonUniqueItems))]
        public void Must_Return_True_WhenThereIsAtLeastOneDuplicate_CaseSensitive(
            IList<string> items)
        {
            var result = target.HasDuplicates(items);

            Assert.IsTrue(result);
        }

        private static IEnumerable<TestCaseData> Get_UniqueItems()
        {
            yield return new TestCaseData(
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "" });

            yield return new TestCaseData(
                new List<string> { null });

            yield return new TestCaseData(
                new List<string> { "", null });

            yield return new TestCaseData(
                new List<string> { "null", null });

            yield return new TestCaseData(
                new List<string> { "one", "two" });

            yield return new TestCaseData(
                new List<string> { "one", "One" });

            yield return new TestCaseData(
                new List<string> { "two", "TWO" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "One" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "One", "", null });

        }

        private static IEnumerable<TestCaseData> Get_NonUniqueItems()
        {
            yield return new TestCaseData(
                new List<string> { "one", "one" });

            yield return new TestCaseData(
                new List<string> { "ZWEI", "ZWEI" });

            yield return new TestCaseData(
                new List<string> { "", "" });

            yield return new TestCaseData(
                new List<string> { null, null });

            yield return new TestCaseData(
                new List<string> { "one", "", "two", "" });

            yield return new TestCaseData(
                new List<string> { "one", "", "two", "three", "two" });
        }
    }
}