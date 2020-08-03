using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class ColumnInserterTests
    {
        Mock<ICellInserter<string>> cellInserter;

        [SetUp]
        public void Setup()
        {
            cellInserter = new Mock<ICellInserter<string>>();
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void Transform__Should_UseThe_CellInserter_ToInsertABrandNewCell_IntoARow(
            IList<string> input,
            int targetColumnIndex)
        {
            var expectedRow = input.ToArray();

            var target = new ColumnInserter(cellInserter.Object, targetColumnIndex, "unique");

            target.Tramsform(expectedRow);

            Verify_AShinyBrandNewCellIsInserted(expectedRow, targetColumnIndex);
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void Transform__Should_Insert_An_Empty_Cell(
           IList<string> input,
           int targetColumnIndex)
        {
            var expectedRow = input.ToArray();

            var target = new ColumnInserter(cellInserter.Object, targetColumnIndex, "unique");

            target.Tramsform(expectedRow);

            Verify_CellInsertion(expectedRow, targetColumnIndex, null);
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void TransformHeader__Should_UseThe_CellInserter_ToInsertABrandNewCell_IntoARow(
            IList<string> input,
            int targetColumnIndex)
        {
            var expectedRow = input.ToArray();

            var target = new ColumnInserter(cellInserter.Object, targetColumnIndex, "unique");

            target.TransformHeader(expectedRow);

            Verify_AShinyBrandNewCellIsInserted(expectedRow, targetColumnIndex);
        }

        [TestCaseSource(nameof(GetTestCases_HeaderCellName))]
        public void TransformHeader__Should_Insert_A_Cell_WithASpecifiedColumnNameValue(
            IList<string> input,
            int targetColumnIndex,
            string columnName)
        {
            var expectedRow = input.ToArray();

            var target = new ColumnInserter(cellInserter.Object, targetColumnIndex, columnName);

            target.TransformHeader(expectedRow);

            Verify_CellInsertion(expectedRow, targetColumnIndex, columnName);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Must_RejectAnEmptyColumnName(
            string columnName)
        {
            Assert.Throws<ArgumentException>(
                () => new ColumnInserter(cellInserter.Object, 1, columnName));
        }

        [TestCaseSource(nameof(GetTestCases_ColumnNameNotUnique))]
        public void TransformHeader__When_AColumnNameIsNotUnique__Must_ThrowAnException(
            IList<string> columnCells,
            string columnName)
        {
            var target = new ColumnInserter(cellInserter.Object, 1, columnName);

            Assert.Throws<NonUniqueColumnException>(
                () => target.TransformHeader(columnCells.ToArray()));
        }

        private void Verify_AShinyBrandNewCellIsInserted(
            string[] expectedRow,
            int targetColumnIndex)
        {
            cellInserter.Verify(
                x => x.Insert(
                    It.Is<string[]>(
                        arg => arg == expectedRow),
                    It.IsAny<int>(),
                    It.IsAny<string>()),
                Times.Once,
                $"Should pass the input on to the cell inserter");

            cellInserter.Verify(
                x => x.Insert(
                    It.Is<string[]>(
                        arg => arg == expectedRow),
                    It.Is<int>(
                        arg => arg == targetColumnIndex),
                    It.IsAny<string>()),
                $"Should pass the target column index on to the cell inserter");
        }

        private void Verify_CellInsertion(
            string[] expectedRow,
            int targetColumnIndex,
            string expectedValue)
        {
            cellInserter.Verify(
                x => x.Insert(
                    It.Is<string[]>(
                        arg => arg == expectedRow),
                    It.Is<int>(
                        arg => arg == targetColumnIndex),
                    It.Is<string>(
                        arg => arg == expectedValue)));
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

        private static IEnumerable<TestCaseData> GetTestCases_HeaderCellName()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                1,
                "new column");

            yield return new TestCaseData(
                new List<string> { "one" },
                0,
                "Somthing else");

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                2,
                "four");
        }

        private static IEnumerable<TestCaseData> GetTestCases_ColumnNameNotUnique()
        {
            yield return new TestCaseData(
                new string[] { "one" },
                "one");

            yield return new TestCaseData(
                new string[] { "one", "two", "three" },
                "two");
        }
    }
}