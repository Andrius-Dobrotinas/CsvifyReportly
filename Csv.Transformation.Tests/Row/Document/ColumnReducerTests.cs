using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class ColumnReducerTests
    {
        Mock<ICellReducer> cellReducer;

        [SetUp]
        public void Setup()
        {
            cellReducer = new Mock<ICellReducer>();
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void Transform__Should_UseThe_CellReducer_ToDoAllTheDirtyWork(
            IList<string> input,
            IList<int> targetColumnIndexes)
        {
            Should_UseThe_CellReducer_ToDoAllTheDirtyWork(input, targetColumnIndexes);
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void TransformHeader__Should_UseThe_CellReducer_ToDoAllTheDirtyWork(
            IList<string> input,
            IList<int> targetColumnIndexes)
        {
            Should_UseThe_CellReducer_ToDoAllTheDirtyWork(input, targetColumnIndexes);
        }

        private void Should_UseThe_CellReducer_ToDoAllTheDirtyWork(
            IList<string> input,
            IList<int> targetColumnIndexes)
        {
            var expectedColumnIndex = targetColumnIndexes.ToArray();
            var expectedRow = input.ToArray();

            var target = new ColumnReducer(expectedColumnIndex, cellReducer.Object);

            var result = target.Tramsform(expectedRow);

            cellReducer.Verify(
                x => x.Reduce(
                    It.Is<string[]>(
                        arg => arg == expectedRow),
                    It.IsAny<int[]>()),
                Times.Once,
                $"Should pass the input on to an instance of {nameof(ICellReducer)}");

            cellReducer.Verify(
                x => x.Reduce(
                    It.Is<string[]>(
                        arg => arg == expectedRow),
                    It.Is<int[]>(
                        arg => arg == expectedColumnIndex)),
                Times.Once,
                $"Should pass the target column indexes on to an instance of {nameof(ICellReducer)} too");
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                new List<int> { 0 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "four" },
                new List<int> { 0, 3 });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three", "four" },
                new List<int> { 0, 2, 3 });
        }
    }
}