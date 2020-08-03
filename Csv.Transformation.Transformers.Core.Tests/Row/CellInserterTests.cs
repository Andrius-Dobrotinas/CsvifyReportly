using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    public class CellInserterTests
    {
        CellInserter<string> target;
        Mock<IArrayElementInserter<string>> elementInserter;

        [SetUp]
        public void Setup()
        {
            elementInserter = new Mock<IArrayElementInserter<string>>();
            target = new CellInserter<string>(elementInserter.Object);
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void Must_UseThe_ArrayElementInserter_ToInsertABrandNew_IntoARow(
            IList<string> row,
            int targetCellIndex)
        {
            var expectedCells = row.ToArray();

            target.Insert(expectedCells, targetCellIndex, null);

            elementInserter.Verify(
                x => x.Insert(
                    It.Is<string[]>(
                        arg => arg == expectedCells),
                    It.IsAny<int>(),
                    It.IsAny<string>()),
                Times.Once,
                "Must pass the source row on to the array element inserter");

            elementInserter.Verify(
                x => x.Insert(
                    It.Is<string[]>(
                        arg => arg == expectedCells),
                    It.Is<int>(
                        arg => arg == targetCellIndex),
                    It.IsAny<string>()),
                "Must pass the target cell index on to the element inserter");
        }

        [TestCase("value")]
        [TestCase("")]
        [TestCase(null)]
        public void Must_PutAValue_IntoTheFreshlyInsertedCell(
            string value)
        {
            var row = new string[0];
            var position = 0;

            target.Insert(row, position, value);

            elementInserter.Verify(
                x => x.Insert(
                    It.Is<string[]>(
                        arg => arg == row),
                    It.Is<int>(
                        arg => arg == position),
                    It.Is<string>(
                        arg => arg == value)));
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_Return_ARowReturnByTheArrayElementInserter(
            IList<string> row)
        {
            var expectedRow = row.ToArray();

            elementInserter.Setup(
                x => x.Insert(
                    It.IsAny<string[]>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(row.ToArray()); //make a separate copy to make sure i spot if it's modified before getting returned

            var result = target.Insert(new string[0], 0, default);

            AssertionExtensions.SequencesAreEqual(expectedRow, result);
        }

        [TestCaseSource(nameof(GetTestCases_BadIndexes))]
        public void When_ATargetCellIndex_IsMoreThanOnePosition_FurtherFromTheLastIndex__Must_ThrowAnException(
            IList<string> input,
            int targetCellIndex)
        {
            Assert.Throws<StructureException>(
                () => target.Insert(input.ToArray(), targetCellIndex, default));
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                1);

            yield return new TestCaseData(
                new List<string> { "one" },
                0);

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                2);
        }

        private static IEnumerable<TestCaseData> Get_Rows()
        {
            yield return new TestCaseData(
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" });
        }

        private static IEnumerable<TestCaseData> GetTestCases_BadIndexes()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                2);

            yield return new TestCaseData(
                new List<string> { "one" },
                5);

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                4);

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                10);
        }
    }
}