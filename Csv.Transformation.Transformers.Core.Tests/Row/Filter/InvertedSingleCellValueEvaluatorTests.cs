using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedSingleCellValueEvaluatorTests
    {
        [TestCaseSource(nameof(GetTestCases_Positive))]
        public void Should_Return_True_ForValuesThatDontMatchTheTargetValue(
            IList<string> row,
            int targetColumnIndex,
            string targetValue)
        {
            var target = new InvertedSingleCellValueEvaluator(targetColumnIndex, targetValue);

            var result = target.IsMatch(row.ToArray());

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(GetTestCases_Negative))]
        public void Should_Return_False_ForValuesThatDoMatchTheTargetValue(
            IList<string> row,
            int targetColumnIndex,
            string targetValue)
        {
            var target = new InvertedSingleCellValueEvaluator(targetColumnIndex, targetValue);

            var result = target.IsMatch(row.ToArray());

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(GetTestCases_NullVsEmpty))]
        public void Should_Treat_NullsAndEmptyStrings_AsDifferentThings(
            IList<string> row,
            int targetColumnIndex,
            string targetValue)
        {
            var target = new InvertedSingleCellValueEvaluator(targetColumnIndex, targetValue);

            var result = target.IsMatch(row.ToArray());

            Assert.IsTrue(result);
        }

        private static IEnumerable<TestCaseData> GetTestCases_Positive()
        {
            yield return new TestCaseData(
                new string[] { "one" },
                0,
                "no=match");

            yield return new TestCaseData(
                new string[] { "one", "two" },
                0,
                "no=match, again");

            yield return new TestCaseData(
                new string[] { "one", "two" },
                0,
                "ONE");

            yield return new TestCaseData(
                new string[] { "one", "two", "three four five", "four" },
                2,
                "you kiddin me?");

            yield return new TestCaseData(
                new string[] { "megadeth" },
                0,
                "");

            yield return new TestCaseData(
                new string[] { "asd", "slayer" },
                1,
                null);
        }

        private static IEnumerable<TestCaseData> GetTestCases_Negative()
        {
            yield return new TestCaseData(
                new string[] { "one" },
                0,
                "one");

            yield return new TestCaseData(
                new string[] { "one", "two" },
                0,
                "one");

            yield return new TestCaseData(
                new string[] { "one", "two", "three" },
                1,
                "two");

            yield return new TestCaseData(
                new string[] { "one", "two", "you kiddin me?", "four" },
                2,
                "you kiddin me?");

            yield return new TestCaseData(
                new string[] { "" },
                0,
                "");

            yield return new TestCaseData(
                new string[] { "asd", null },
                1,
                null);
        }

        private static IEnumerable<TestCaseData> GetTestCases_NullVsEmpty()
        {
            yield return new TestCaseData(
                new string[] { "" },
                0,
                null);

            yield return new TestCaseData(
                new string[] { null },
                0,
                "");
        }
    }
}