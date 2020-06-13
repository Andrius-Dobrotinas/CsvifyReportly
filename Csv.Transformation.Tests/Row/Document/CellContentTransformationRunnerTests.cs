using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class CellContentTransformationRunnerTests
    {
        CellContentTransformationRunner target;
        Mock<Row.IRowTransformationRunner<ICellContentTransformer>> rowTransformationRunner;
        Mock<ICellContentTransformer> transformer;

        [SetUp]
        public void Setup()
        {
            rowTransformationRunner = new Mock<Row.IRowTransformationRunner<ICellContentTransformer>>();
            target = new CellContentTransformationRunner(rowTransformationRunner.Object);

            transformer = new Mock<ICellContentTransformer>();
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_TransformTheRowsOnly_UsingTheSuppliedTransformer(
            IList<string[]> rows)
        {
            var expectedRows = rows.ToArray();

            var document = new CsvDocument
            {
                Rows = expectedRows,
                ColumnNames = new string[0]
            };

            target.Transform(document, transformer.Object);

            rowTransformationRunner.Verify(
                x => x.TransformRows(
                    It.Is<ICellContentTransformer>(
                        arg => arg == transformer.Object),
                    It.IsAny<string[][]>()),
                    Times.Once,
                    "Must use the transformation runner and pass the supplied transformer on to it");

            rowTransformationRunner.Verify(
                x => x.TransformRows(
                    It.Is<ICellContentTransformer>(
                        arg => arg == transformer.Object),
                    It.Is<string[][]>(
                        arg => arg == expectedRows)),
                    Times.Once,
                    "Must pass the CSV rows on to the function");
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_ReturnTheTransformedRows(
            IList<string[]> transfomedRows)
        {
            var expectedRows = transfomedRows.ToArray();
            Setup_TransformationRunner(expectedRows);

            var document = new CsvDocument
            {
                Rows = new string[0][],
                ColumnNames = new string[0]
            };

            var result = target.Transform(document, transformer.Object);

            Assert.AreSame(expectedRows, result.Rows);
        }

        [TestCaseSource(nameof(Get_ColumnNames))]
        public void Must_ReturnTheOriginalColumnsCollection(
            IList<string> columnNames)
        {
            Setup_TransformationRunner(new string[0][]);

            var document = new CsvDocument
            {
                Rows = new string[0][],
                ColumnNames = columnNames.ToArray()
            };

            /* a different instance from that in the CsvDocument in order to make sure that if
             * the collection is changed by the target, the expected value is not, and therefore
             * the test is not falsely positive */
            var expectedColumns = columnNames.ToArray();

            var result = target.Transform(document, transformer.Object);

            Assert.AreEqual(expectedColumns, result.ColumnNames);
        }

        private void Setup_TransformationRunner(string[][] returnValue)
        {
            rowTransformationRunner.Setup(
                x => x.TransformRows(
                    It.IsAny<ICellContentTransformer>(),
                    It.IsAny<string[][]>()))
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