using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Collections
{
    public class WritingEnumeratorTests
    {
        WritingEnumerator<string> target;

        [TestCaseSource(nameof(Get_Values))]
        public void AddingItems__When_SourceIsEmptyInitially__Must_AddItemAtTheEndOfCollection(IList<string> values)
        {
            var source = new List<string>();

            target = new WritingEnumerator<string>(source);

            foreach (var value in values)
                target.Add(value);

            AssertionExtensions.SequencesAreEqual(values, source);
        }

        [TestCaseSource(nameof(Get_Values_WithNonEmptyInitialSource))]
        public void AddingItems__When_SourceIsNotEmptyInitially__Must_AddItemAtTheEndOfCollection(
            IList<string> source,
            IList<string> values)
        {
            var expectedCollection = source.Concat(values).ToList();

            target = new WritingEnumerator<string>(source);

            foreach (var value in values)
                target.Add(value);

            AssertionExtensions.SequencesAreEqual(expectedCollection, source);
        }

        [Test]
        public void GetCurrent__When_SourceIsEmpty_Must_ThrowAnException()
        {
            target = new WritingEnumerator<string>(new List<string>());

            Assert.Throws<ArgumentOutOfRangeException>(
                () => { var current = target.Current; });
        }

        [TestCaseSource(nameof(Get_Values))]
        public void IteratingOverItems__When_SourceIsNotEmpty_ShouldReturnAllItemsInTheSource(IList<string> values)
        {
            target = new WritingEnumerator<string>(values);

            foreach (var value in values)
            {
                target.MoveNext();
                Assert.AreEqual(value, target.Current);
            }
        }

        [TestCaseSource(nameof(Get_Values))]
        public void Current__When_SourceIsEmptyInitially_WhenAnItemIsAdded_TheItemMustBeTheCurrentItem(
            IList<string> values)
        {
            target = new WritingEnumerator<string>(new List<string>());

            foreach (var value in values)
            {
                target.Add(value);
                Assert.AreEqual(value, target.Current);
            }
        }

        [TestCaseSource(nameof(Get_Values_WithNonEmptyInitialSource))]
        public void Current__When_SourceIsNotEmptyInitially_WhenAnItemIsAdded_TheItemMustBeTheCurrentItem(
            IList<string> source,
            IList<string> values)
        {
            target = new WritingEnumerator<string>(values);

            foreach (var value in values)
            {
                target.MoveNext();
                Assert.AreEqual(value, target.Current);
            }
        }

        [TestCaseSource(nameof(Get_Values))]
        public void FullEnumeration__When_SourceIsEmptyInitially_When_ItemsAreAdded__AndPositionIsReset__MustReturnEveryItem_InTheOriginalSequence(
            IList<string> values)
        {
            var source = new List<string>();

            target = new WritingEnumerator<string>(source);

            foreach (var value in values)
                target.Add(value);

            target.ResetPosition();

            foreach (var value in values)
            {
                target.MoveNext();
                Assert.AreEqual(value, target.Current);
            }
        }

        [TestCaseSource(nameof(Get_Values_WithNonEmptyInitialSource))]
        public void FullEnumeration__When_SourceIsNotEmptyInitially_When_ItemsAreAdded__AndPositionIsReset__MustReturnEveryItem_InTheOriginalSequence(
            IList<string> source,
            IList<string> values)
        {
            var expectedCollection = source.Concat(values).ToList();

            target = new WritingEnumerator<string>(source);

            foreach (var value in values)
                target.Add(value);

            target.ResetPosition();

            foreach (var value in expectedCollection)
            {
                target.MoveNext();
                Assert.AreEqual(value, target.Current);
            }
        }

        private static IEnumerable<TestCaseData> Get_Values()
        {
            yield return new TestCaseData(
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "two" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "four" });
        }

        private static IEnumerable<TestCaseData> Get_Values_WithNonEmptyInitialSource()
        {
            yield return new TestCaseData(
                new List<string> { "initial" },
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "initial1", "initial2" },
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "initial1" },
                new List<string> { "one", "two" });

            yield return new TestCaseData(
                new List<string> { "initial1", "initial2", "initial3" },
                new List<string> { "one", "two", "three", "four", "five" });
        }
    }
}