using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Collections
{
    public class SmartnumeratorTests
    {
        Smartnumerator<string> target;

        private IEnumerable<string> CreateOneTimeEnumerable(IEnumerable<string> sourceEnumerable)
        {
            return new OneTimeEnumerableSource<string>(sourceEnumerable)
                .GetEnumerable();
        }

        private IEnumerable<string> BuildEnumerableWithSmartnumerator(IEnumerable<string> sourceEnumerable)
        {
            target = new Smartnumerator<string>(sourceEnumerable.GetEnumerator());
            var fake = new Mock<IEnumerable<string>>();
            fake.Setup(x => x.GetEnumerator()).Returns(target);

            return fake.Object;
        }

        [Test]
        public void PreTest_MakeSureMyTestEnumerableDoesNotSupportMultipleEnumerations()
        {
            var originalEnumerable = CreateOneTimeEnumerable(new string[] { null });

            originalEnumerable.ToArray();

            for (int i = 0; i < 3; i++)
            {
                Assert.Throws<MultipleEnumerationNotSupportedException>(
                    () => originalEnumerable.ToArray(),
                    "Must making sure that the original enumerable doesn't support multiple enumerations");
            }
        }

        [TestCaseSource(nameof(GetEnumerables))]
        public void OnFirstEnumeration_Must_SuccessfullyEnumerateTheSourceCollection(IEnumerable<string> sourceEnumerable)
        {
            var originalEnumerable = CreateOneTimeEnumerable(sourceEnumerable);
            var smartnumerable = BuildEnumerableWithSmartnumerator(originalEnumerable);

            var result1 = smartnumerable.ToArray();

            AssertionExtensions.SequencesAreEqual(sourceEnumerable, result1);
        }

        [Test]
        public void OnFirstEnumeration_When_SourceIsEmpty_Must_DoTheSameAsWhenItsNotEmpty()
        {
            var source = new List<string>(0);
            OnFirstEnumeration_Must_SuccessfullyEnumerateTheSourceCollection(source);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void AfterFirstCompleteEnumeration_OnSubsequentEnumerations_Must_ReturnAllTheValuesWithoutTalkingToTheOriginalCollection(int subsequentEnumerationCount)
        {
            var sourceEnumerable = new string[] { "ein", null, "drei" };

            AfterFirstCompleteEnumeration_OnSubsequentEnumerations_Must_ReturnAllTheValuesWithoutTalkingToTheOriginalCollection(
                sourceEnumerable,
                subsequentEnumerationCount);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void AfterFirstCompleteEnumeration_OnSubsequentEnumerations_When_SourceIsEmpty__Must_DoTheSameAsWhenItsNotEmpty(
            int subsequentEnumerationCount)
        {
            var source = new List<string>(0);
            AfterFirstCompleteEnumeration_OnSubsequentEnumerations_Must_ReturnAllTheValuesWithoutTalkingToTheOriginalCollection(
                source,
                subsequentEnumerationCount);
        }

        private void AfterFirstCompleteEnumeration_OnSubsequentEnumerations_Must_ReturnAllTheValuesWithoutTalkingToTheOriginalCollection(
            IEnumerable<string> sourceEnumerable,
            int subsequentEnumerationCount)
        {
            var originalEnumerable = CreateOneTimeEnumerable(sourceEnumerable);
            var enumerable = BuildEnumerableWithSmartnumerator(originalEnumerable);

            enumerable.ToArray();

            for (int i = 0; i < subsequentEnumerationCount; i++)
            {
                var result = enumerable.ToArray();

                AssertionExtensions.SequencesAreEqual(sourceEnumerable, result);
            }
        }

        [TestCaseSource(nameof(Get_Enumerables_ForIncompleteEnumerations_ThatDontGoFarthterThanThe1stOne))]
        public void WhenFirstEnumerationIs_NotCompleted_OnSubsequentEnumerations_Must_ReturnCachedValuesFromTheCopy(
            IEnumerable<string> sourceEnumerable,
            int itemsToTakeFirstTime,
            int[] itemsToTakeOnSubsequentEnumerations)
        {
            if (itemsToTakeOnSubsequentEnumerations.Max() > itemsToTakeFirstTime)
                throw new ArgumentException();
            RunMultipleEnumerations_ExpectToGetSameResultsThatTheEnumerationOfTheTestSourceEnumerableYields(
                sourceEnumerable,
                itemsToTakeFirstTime,
                itemsToTakeOnSubsequentEnumerations);
        }

        [TestCaseSource(nameof(Get_EnumerablesFor_IncompleteEnumerations_WhereSubsequentOneGoesFartherThanThe1stOne))]
        public void WhenFirstEnumerationIs_NotCompleted_OnSubsequentEnumerationsThatGoFarther_Must_ReturnCachedValuesFromTheCopy_AndThenGetTheRemainingValuesFromTheSourceEnumerable(
            IEnumerable<string> sourceEnumerable,
            int itemsToTakeFirstTime,
            int[] itemsToTakeOnSubsequentEnumerations)
        {
            RunMultipleEnumerations_ExpectToGetSameResultsThatTheEnumerationOfTheTestSourceEnumerableYields(
                sourceEnumerable,
                itemsToTakeFirstTime,
                itemsToTakeOnSubsequentEnumerations);
        }

        private void RunMultipleEnumerations_ExpectToGetSameResultsThatTheEnumerationOfTheTestSourceEnumerableYields(
            IEnumerable<string> sourceEnumerable,
            int itemsToTakeFirstTime,
            int[] itemsToTakeOnSubsequentEnumerations)
        {
            var originalEnumerable = CreateOneTimeEnumerable(sourceEnumerable);
            var enumerable = BuildEnumerableWithSmartnumerator(originalEnumerable);

            //incomplete first enumeration
            enumerable.Take(itemsToTakeFirstTime).ToList();

            //subsequent enumerations
            for (int i = 0; i < itemsToTakeOnSubsequentEnumerations.Length; i++)
            {
                int itemsToTakeOnSubsequentEnumeration = itemsToTakeOnSubsequentEnumerations[i];

                var expectedCollection = sourceEnumerable.Take(itemsToTakeOnSubsequentEnumeration).ToList();

                var result = enumerable.Take(itemsToTakeOnSubsequentEnumeration).ToList();

                AssertionExtensions.SequencesAreEqual(expectedCollection, result, $"Enumeration {i}");
            }
        }

        private static IEnumerable<TestCaseData> GetEnumerables()
        {
            yield return new TestCaseData(
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "two" });

            yield return new TestCaseData(
                new List<string> { "ein", "zwei", "drei", "vier", "funf" });
        }

        private static IEnumerable<TestCaseData> Get_Enumerables_ForIncompleteEnumerations_ThatDontGoFarthterThanThe1stOne()
        {
            yield return new TestCaseData(
                new List<string> { "one", "two" },
                1,
                new[] { 1 });

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                1,
                new[] { 1, 1 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                2,
                new int[] { 2 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                2,
                new int[] { 2, 2 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                2,
                new int[] { 2, 1 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                2,
                new int[] { 2, 1, 2 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                2,
                new int[] { 2, 1, 2, 1, 2 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "ein", "zwei" },
                3,
                new int[] { 3 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "ein", "zwei" },
                3,
                new int[] { 3, 2, 1, 3 });
        }

        private static IEnumerable<TestCaseData> Get_EnumerablesFor_IncompleteEnumerations_WhereSubsequentOneGoesFartherThanThe1stOne()
        {
            yield return new TestCaseData(
                new List<string> { "one", "two" },
                1,
                new int[] { 2 });

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                1,
                new int[] { 2, 2 });

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                1,
                new int[] { 2, 1, 2 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                2,
                new int[] { 3 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                2,
                new int[] { 1, 2, 3 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "ein", "zwei" },
                2,
                new int[] { 4 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "ein", "zwei" },
                2,
                new int[] { 3, 4, 5 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "ein", "zwei" },
                2,
                new int[] { 5 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "ein", "zwei" },
                2,
                new int[] { 5, 2, 4, 5 });
        }
    }

    public class OneTimeEnumerableSource<T>
    {
        private int enumrationCount = 0;
        private readonly IEnumerable<T> source;

        public OneTimeEnumerableSource(IEnumerable<T> source)
        {
            this.source = source;
        }

        public IEnumerable<T> GetEnumerable()
        {
            if (++enumrationCount > 1)
                throw new MultipleEnumerationNotSupportedException();

            foreach (var item in source)
                yield return item;
        }
    }

    public class MultipleEnumerationNotSupportedException : Exception
    {
        public MultipleEnumerationNotSupportedException()
            : base("Can't enumerate more than once!")
        {
        }
    }
}