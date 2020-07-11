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
        Mock<IEnumerable<string>> mockEnumerable;
        Mock<IEnumerable<string>> fake;

        private IEnumerable<string> BuildEnumerableWithSmartnumerator(IEnumerable<string> sourceEnumerable)
        {
            mockEnumerable = new Mock<IEnumerable<string>>();
            mockEnumerable.SetupSequence(x => x.GetEnumerator())
                .Returns(sourceEnumerable.GetEnumerator())
                .Throws<Exception>();

            target = new Smartnumerator<string>(sourceEnumerable.GetEnumerator());
            fake = new Mock<IEnumerable<string>>();
            fake.Setup(x => x.GetEnumerator()).Returns(target);

            return fake.Object;
        }

        [Test]
        public void PreTest_MakeSureMySourceEnumerableDoesNotSupportMultipleEnumerations()
        {
            BuildEnumerableWithSmartnumerator(new string[] { null });
            
            mockEnumerable.Object.ToArray();

            Assert.Throws<Exception>(() => mockEnumerable.Object.ToArray(),
                "Must making sure that the original enumerable doesn't support multiple enumerations");
        }

        [TestCaseSource(nameof(GetSource))]
        public void OnFirstEnumeration_Must_SuccessfullyEnumerateTheSourceCollection(IEnumerable<string> sourceEnumerable)
        {
            var enumerable = BuildEnumerableWithSmartnumerator(sourceEnumerable);

            var result1 = enumerable.ToArray();

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
            var enumerable = BuildEnumerableWithSmartnumerator(sourceEnumerable);

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
}