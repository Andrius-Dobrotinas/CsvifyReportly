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
        public void PreTest_MakeSure_MyTestEnumerable_DoesNotSupportMultipleEnumerations()
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

        [TestCaseSource(nameof(GetSource))]
        public void OnFirstEnumeration_Must_SuccessfullyEnumerateTheSourceCollection(IEnumerable<string> sourceEnumerable)
        {
            var originalEnumerable = CreateOneTimeEnumerable(sourceEnumerable);
            var smartnumerable = BuildEnumerableWithSmartnumerator(originalEnumerable);

            var result1 = smartnumerable.ToArray();

            AssertionExtensions.SequencesAreEqual(sourceEnumerable, result1);
        }

        [Test]
        public void When_SourceIsEmpty__OnFirstEnumeration_Must_DoTheSameAsWhenItsNotEmpty()
        {
            var source = new List<string>(0);
            OnFirstEnumeration_Must_SuccessfullyEnumerateTheSourceCollection(source);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void OnSubsequentEnumerations_Must_ReturnAllTheValuesWithoutTalkingToTheOriginalCollection(int subsequentEnumerationCount)
        {
            var sourceEnumerable = new string[] { "ein", null, "drei" };

            OnSubsequentEnumerations_Must_ReturnAllTheValuesWithoutTalkingToTheOriginalCollection(
                sourceEnumerable,
                subsequentEnumerationCount);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void When_SourceIsEmpty_OnSubsequentEnumerations__Must_DoTheSameAsWhenItsNotEmpty(
            int subsequentEnumerationCount)
        {
            var source = new List<string>(0);
            OnSubsequentEnumerations_Must_ReturnAllTheValuesWithoutTalkingToTheOriginalCollection(
                source,
                subsequentEnumerationCount);
        }

        private void OnSubsequentEnumerations_Must_ReturnAllTheValuesWithoutTalkingToTheOriginalCollection(
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

        private static IEnumerable<TestCaseData> GetSource()
        {
            yield return new TestCaseData(
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "two" });

            yield return new TestCaseData(
                new List<string> { "ein", "zwei", "drei", "vier", "funf" });
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