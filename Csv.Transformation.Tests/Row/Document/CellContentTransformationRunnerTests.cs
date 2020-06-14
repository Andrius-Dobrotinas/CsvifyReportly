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
        Mock<IRowTransformationRunner<IRowTransformer>> rowTransformationRunner;
        Mock<ICellContentTransformer> transformer;

        [SetUp]
        public void Setup()
        {
            rowTransformationRunner = new Mock<IRowTransformationRunner<IRowTransformer>>();
            target = new CellContentTransformationRunner(rowTransformationRunner.Object);

            transformer = new Mock<ICellContentTransformer>();
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_InvokeAMethodOn_TransformationRunner_Once_InOrderToTransformDocumentRows(
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
                    It.IsAny<ICellContentTransformer>(),
                    It.IsAny<string[][]>(),
                    It.IsAny<int>()),
                Times.Once,
                "Must use the transformation runner to do its dirty work once");
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_PassTheDocumentRows_OnToTheTransformationRunner(
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
                    It.IsAny<ICellContentTransformer>(),
                    It.Is<string[][]>(
                        arg => arg == expectedRows),
                    It.IsAny<int>()),
                "Must pass the document rows on to the transformer");
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_PassTheProvidedTransformer_OnToTheTransformationRunner(
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
                    It.IsAny<string[][]>(),
                    It.IsAny<int>()));
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_PassThe_TheLengthOfTheFirstRow_AsTheExpectedRowLength(
            IList<string[]> rows)
        {
            var expectedRowLength = rows.First().Length;

            var document = new CsvDocument
            {
                Rows = rows.ToArray(),
                ColumnNames = new string[0]
            };

            target.Transform(document, transformer.Object);

            rowTransformationRunner.Verify(
                x => x.TransformRows(
                    It.IsAny<ICellContentTransformer>(),
                    It.IsAny<string[][]>(),
                    It.Is<int>(
                        arg => arg == expectedRowLength)));
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_Return_TheTransformedRows(
            IList<string[]> transfomedRows)
        {
            var expectedRows = transfomedRows.ToArray();
            Setup_TransformationRunner(expectedRows);

            var document = new CsvDocument
            {
                Rows = new string[][] { new string[] { "cell" } },
                ColumnNames = new string[0]
            };

            var result = target.Transform(document, transformer.Object);

            Assert.AreSame(expectedRows, result.Rows);
        }

        [Test]
        public void When_TheRowsCollectionIsEmpty__Must_Return_ItRightAway()
        {
            var document = new CsvDocument
            {
                Rows = new string[0][],
                ColumnNames = new string[0]
            };

            var result = target.Transform(document, transformer.Object);

            Assert.IsEmpty(result.Rows);
        }

        [TestCaseSource(nameof(Get_ColumnNames))]
        public void Must_Return_TheOriginalColumnsCollection(
            IList<string> columnNames)
        {
            Must_Return_TheOriginalColumnsCollection(
                columnNames,
                new string[][]
                {
                    new string [] { "geen" }
                });
        }

        [TestCaseSource(nameof(Get_ColumnNames))]
        public void Must_Return_TheOriginalColumnsCollection_WhenTheRowsCollectionIsEmpty(
            IList<string> columnNames)
        {
            Must_Return_TheOriginalColumnsCollection(columnNames, new string[0][]);
        }

        private void Must_Return_TheOriginalColumnsCollection(
            IList<string> columnNames,
            string[][] rows)
        {
            Setup_TransformationRunner(new string[0][]);

            var document = new CsvDocument
            {
                Rows = rows,
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
                    It.IsAny<string[][]>(),
                    It.IsAny<int>()))
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