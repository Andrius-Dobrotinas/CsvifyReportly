using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class RowFilterRunnerTests
    {
        RowFilterRunner target;
        Mock<Filtering.IRowFilter> rowFilter;
        Mock<Filtering.IRowMatchEvaluator> rowMatchEvaluator;

        [SetUp]
        public void Setup()
        {
            rowFilter = new Mock<Filtering.IRowFilter>();
            rowMatchEvaluator = new Mock<Filtering.IRowMatchEvaluator>();

            target = new RowFilterRunner(rowFilter.Object);
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_FilterOutTheRows_UsingTheLowerLevelRowFilter(
            IList<string[]> rows)
        {
            var expectedRows = rows.ToArray();

            var document = new CsvDocument
            {
                ContentRows = expectedRows,
                HeaderCells = new string[0]
            };

            target.Transform(document, rowMatchEvaluator.Object);

            rowFilter.Verify(
                x => x.Filter(
                    It.Is<string[][]>(
                        arg => arg == expectedRows),
                    It.IsAny<Filtering.IRowMatchEvaluator>()),
                Times.Once,
                "Must run use the filter component, and pass the rows onto it");

            rowFilter.Verify(
                x => x.Filter(
                    It.IsAny<string[][]>(),
                    It.Is<Filtering.IRowMatchEvaluator>(
                        arg => arg == rowMatchEvaluator.Object)),
                "Must run pass the match evaluator on to the filter component");
        }

        [TestCaseSource(nameof(Get_ColumnNames))]
        public void Must_Return_TheOriginalColumnsCollection(
            IList<string> columnNames)
        {
            Must_Return_TheOriginalColumnsCollection(
                columnNames,
                new string[][]
                {
                    new string[] { "black" },
                    new string [] { "green" }
                },
                new string[][]
                {
                    new string [] { "green" }
                });
        }

        [TestCaseSource(nameof(Get_ColumnNames))]
        public void Must_Return_TheOriginalColumnsCollection_WhenResultOfRowFilterionIsEmpty(
            IList<string> columnNames)
        {
            Must_Return_TheOriginalColumnsCollection(columnNames, new string[0][], new string[0][]);
        }
        
        private void Must_Return_TheOriginalColumnsCollection(
            IList<string> columnNames,
            string[][] rows,
            string[][] expectedRows)
        {
            Setup_Filter(expectedRows);

            var document = new CsvDocument
            {
                ContentRows = rows,
                HeaderCells = columnNames.ToArray()
            };

            /* a different instance from that in the CsvDocument in order to make sure that if
             * the collection is changed by the target, the expected value is not, and therefore
             * the test is not falsely positive */
            var expectedColumns = columnNames.ToArray();

            var result = target.Transform(document, rowMatchEvaluator.Object);

            Assert.AreEqual(expectedColumns, result.HeaderCells);
        }

        private void Setup_Filter(IEnumerable<string[]> returnValue)
        {
            rowFilter.Setup(
                x => x.Filter(
                    It.IsAny<string[][]>(),
                    It.IsAny<Filtering.IRowMatchEvaluator>()))
                .Returns(returnValue);
        }

        private static IEnumerable<TestCaseData> Get_Rows()
        {
            yield return new TestCaseData(
                new List<string[]> { 
                    new string[] { "one" }
                });

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one", "one two" },
                    new string[] { "two", "two two" },
                    new string[] { "three", "three two" }
                });
        }
        
        private static IEnumerable<TestCaseData> Get_ColumnNames()
        {
            yield return new TestCaseData(
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" });
        }
    }
}