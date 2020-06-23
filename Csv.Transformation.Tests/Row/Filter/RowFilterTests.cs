using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class RowFilterTests
    {
        RowFilter target = new RowFilter();

        [TestCaseSource(nameof(GetRows))]
        public void Must_Invoke_TheMatchEvaluator_ForEachRow(
            IList<string[]> rows)
        {
            var matcher = new Mock<IRowMatchEvaluator>();

            target.Filter(rows, matcher.Object).ToArray();

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                matcher.Verify(
                    x => x.IsMatch(
                        It.Is<string[]>(
                            arg => arg == row)),
                    Times.Once,
                    $"Must invoke the match evaluator with each row - once (current row {i})");
            }
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void Must_Return_OnlyTheRowsTheEvaluatedDeemedWorthyOfBeingReturned(
            IList<string[]> rows,
            IList<bool> matches,
            IList<string[]> expectedRows)
        {
            var matcher = new Mock<IRowMatchEvaluator>();
            Setup_MatchEvaluator(matcher, matches);

            var result = target.Filter(rows, matcher.Object).ToArray();

            AssertionExtensions.SequencesAreEqual(expectedRows, result);
        }

        private void Setup_MatchEvaluator(Mock<IRowMatchEvaluator> matcher, IEnumerable<bool> returnValueSequence)
        {
            var sequence = matcher.SetupSequence(
                    x => x.IsMatch(
                        It.IsAny<string[]>()));

            foreach (var value in returnValueSequence)
                sequence.Returns(value);
        }

        private static IEnumerable<TestCaseData> GetRows()
        {
            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one" }
                });

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one" },
                    new string[] { "one", "two", "three" },
                    new string[] { "one", "two" },
                });
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            var row1 = new string[] { "one" };
            var row2 = new string[] { "one", "two", "three" };
            var row3 = new string[] { "ein", "zwei", "drei" };

            yield return new TestCaseData(
                new List<string[]> { row1 },
                new List<bool> { true },
                new List<string[]> { row1 });

            yield return new TestCaseData(
                new List<string[]> { row1 },
                new List<bool> { false },
                new List<string[]> { });

            yield return new TestCaseData(
                new List<string[]> { row1, row2, row3 },
                new List<bool> { true, true, true },
                new List<string[]> { row1, row2, row3 });

            yield return new TestCaseData(
                new List<string[]> { row1, row2, row3 },
                new List<bool> { false, false, false },
                new List<string[]> { });

            yield return new TestCaseData(
                new List<string[]> { row1, row2, row3 },
                new List<bool> { true, false, true },
                new List<string[]> { row1, row3 });

            yield return new TestCaseData(
                new List<string[]> { row1, row2, row3 },
                new List<bool> { false, true, true },
                new List<string[]> { row2, row3 });

            yield return new TestCaseData(
                new List<string[]> { row1, row2, row3 },
                new List<bool> { false, true, false },
                new List<string[]> { row2 });
        }
    }
}