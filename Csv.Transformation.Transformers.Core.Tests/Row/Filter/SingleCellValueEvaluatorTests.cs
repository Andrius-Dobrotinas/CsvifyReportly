using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class SingleCellValueEvaluatorTests
    {
        Mock<IValueComparer> valueComparer;

        [TestCaseSource(nameof(GetTestCases))]
        public void Should_CompareValueFromTheTargetCell(
            IList<string> row,
            int targetColumnIndex,
            string targetValue)
        {
            var target = CreateTarget(targetColumnIndex);

            var result = target.IsMatch(row.ToArray());

            valueComparer.Verify(
                x => x.IsMatch(
                    It.Is<string>(
                        arg => arg == targetValue)),
                Times.Once);
        }

        [TestCaseSource(nameof(GetRows))]
        public void Should_Return_False_ForValuesThatDontMatchTheTargetValue(
            IList<string> row)
        {
            var target = CreateTarget(0);

            Setup_ValueComparer(false);

            var result = target.IsMatch(row.ToArray());

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(GetRows))]
        public void Should_Return_True_ForValuesThatDoMatchTheTargetValue(
            IList<string> row)
        {
            var target = CreateTarget(0);

            Setup_ValueComparer(true);

            var result = target.IsMatch(row.ToArray());

            Assert.IsTrue(result);
        }

        public SingleCellValueEvaluator CreateTarget(int targetColumnIndex)
        {
            valueComparer = new Mock<IValueComparer>();

            return new SingleCellValueEvaluator(targetColumnIndex, valueComparer.Object);
        }

        private void Setup_ValueComparer(bool returnValue)
        {
            valueComparer.Setup(
                x => x.IsMatch(
                    It.IsAny<string>()))
                .Returns(returnValue);
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(
                new string[] { "oNe", "two" },
                0,
                "oNe");

            yield return new TestCaseData(
                new string[] { "one", "two", "three four five", "four" },
                2,
                "three four five");

            yield return new TestCaseData(
                new string[] { "one", "two", "three four five", "four" },
                3,
                "four");
        }

        private static IEnumerable<TestCaseData> GetRows()
        {
            yield return new TestCaseData(new List<string> { "one" });

            yield return new TestCaseData(new List<string> { "one", "two", "three" });
        }
    }
}