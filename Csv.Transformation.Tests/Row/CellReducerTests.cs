using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    public class CellReducerTests
    {
        CellReducer target = new CellReducer();

        [TestCaseSource(nameof(GetTestCases))]
        public void Should_OnlyReturnTargetColumns(
            IList<string> input,
            IList<int> targetColumnIndexes,
            IList<string> expectedResult)
        {
            var result = target.Reduce(input.ToArray(), targetColumnIndexes.ToArray());

            Assert.IsTrue(expectedResult.SequenceEqual(result));
        }

        [TestCaseSource(nameof(GetTestCases_TargetIndexOutOfBounds))]
        public void When_AnyTargetIndexIsOutsideTheBoundsOfTheRow__Must_ThrowAnException(
            IList<string> input,
            IList<int> targetCellIndexes)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => target.Reduce(input.ToArray(), targetCellIndexes.ToArray()));
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                new List<int> { 0 },
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "four" },
                new List<int> { 0, 1, 2, 3 },
                new List<string> { "one", "two", "three", "four" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "four" },
                new List<int> { 0 },
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "four" },
                new List<int> { 0, 1 },
                new List<string> { "one", "two" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "four" },
                new List<int> { 0, 1, 2 },
                new List<string> { "one", "two", "three" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "four" },
                new List<int> { 3 },
                new List<string> { "four" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "four" },
                new List<int> { 1, 2 },
                new List<string> { "two", "three" });
        }

        private static IEnumerable<TestCaseData> GetTestCases_TargetIndexOutOfBounds()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                new List<int> { 1 });

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                new List<int> { 0, 4, 1 });

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                new List<int> { 0, 4, 5 });
        }
    }
}