using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv
{
    public class ArrayElementInserterTests
    {
        ArrayElementInserter<string> target = new ArrayElementInserter<string>();


        [TestCaseSource(nameof(GetTestCases_ElementBetweenExistingElements))]
        public void Should_InsertAValue_BetweenExistingArrayElements(
            string[] input,
            int targetPosition,
            string value,
            string[] expectedResult)
        {
            var result = target.Insert(input, targetPosition, value);
            
            AssertionExtensions.SequencesAreEqual(expectedResult, result);
        }

        [TestCaseSource(nameof(GetTestCases_ElementIsFirst))]
        public void Should_InsertAValue_WhenItsTheFirstElementInTheArray(
            string[] input,
            string value,
            string[] expectedResult)
        {
            var result = target.Insert(input, 0, value);

            AssertionExtensions.SequencesAreEqual(expectedResult, result);
        }

        [TestCaseSource(nameof(GetTestCases_ElementIsLast))]
        public void Should_InsertAValue_AtTheEndOfArray(
            string[] input,
            string value,
            string[] expectedResult)
        {
            var result = target.Insert(input, input.Length, value);

            AssertionExtensions.SequencesAreEqual(expectedResult, result);
        }

        private static IEnumerable<TestCaseData> GetTestCases_ElementBetweenExistingElements()
        {
            yield return new TestCaseData(
                new string[] { "one", "two" },
                1,
                "NEW VAL",
                new string[] { "one", "NEW VAL", "two" });

            yield return new TestCaseData(
                new string[] { "one", "two", "three" },
                2,
                "NEW VAL TOO",
                new string[] { "one", "two", "NEW VAL TOO", "three" });
        }

        private static IEnumerable<TestCaseData> GetTestCases_ElementIsFirst()
        {
            yield return new TestCaseData(
                new string[] { "one" },
                "NEW VAL",
                new string[] { "NEW VAL", "one" });

            yield return new TestCaseData(
                new string[] { "one", "two", "three" },
                "NEW VAL AGAIN",
                new string[] { "NEW VAL AGAIN", "one", "two", "three" });
        }

        private static IEnumerable<TestCaseData> GetTestCases_ElementIsLast()
        {
            yield return new TestCaseData(
                new string[] { "one" },
                "NEW VAL",
                new string[] { "one", "NEW VAL" });

            yield return new TestCaseData(
                new string[] { "one", "two", "three" },
                "NEW VAL AGAIN",
                new string[] { "one", "two", "three", "NEW VAL AGAIN" });

            yield return new TestCaseData(
                new string[] { },
                "NEW VAL",
                new string[] { "NEW VAL" });
        }
    }
}