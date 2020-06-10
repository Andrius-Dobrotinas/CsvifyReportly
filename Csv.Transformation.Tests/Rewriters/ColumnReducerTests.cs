using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Rewriters
{
    public class ColumnReducerTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public void Should_OnlyReturnTargetColumns(
            IList<string> input,
            IList<int> targetColumnIndexes,
            IList<string> expectedResult)
        {
            var target = new ColumnReducer(targetColumnIndexes.ToArray());

            var result = target.Rewrite(input.ToArray());

            Assert.IsTrue(expectedResult.SequenceEqual(result));
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
    }
}