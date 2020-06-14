using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class StructureTransformationRunnerTests
    {
        StructureTransformationRunner target;
        Mock<IRowTransformationRunner<IRowTransformer>> rowTransformationRunner;
        Mock<IStructureTransformer> transformer;

        [SetUp]
        public void Setup()
        {
            rowTransformationRunner = new Mock<IRowTransformationRunner<IRowTransformer>>();
            target = new StructureTransformationRunner(rowTransformationRunner.Object);

            transformer = new Mock<IStructureTransformer>();

            Setup_HeaderTransformation(new string[0]);
        }

        [TestCaseSource(nameof(Get_HeaderCells))]
        public void Must_FirstTransformHeader_RegardlessOfWhetherThereAreColumnsDefined(
            IList<string> columns)
        {
            //i want to check that Header was transformed before it attempted to transform the content
            GetSetup_TransformationRunner()
                .Throws<NonWelcomeInvocatioException>();

            var document = new CsvDocument
            {
                Rows = new string[][] { new string[] { "one" } },
                ColumnNames = columns.ToArray()
            };

            try
            {
                target.Transform(document, transformer.Object);
            }
            catch(NonWelcomeInvocatioException)
            {
            }

            transformer.Verify(
                x => x.TransformHeader(
                    It.IsAny<string[]>()),
                Times.Once,
                "Must transform the Header before transforming the content");
        }

        [TestCaseSource(nameof(Get_HeaderCells))]
        public void Must_PassTheColumnCellsOnToThe_HeaderTransformer(
            IList<string> columns)
        {
            var expectedHeaderCells = columns.ToArray();
            var document = new CsvDocument
            {
                Rows = new string[][] { new string[] { "cell one" } },
                ColumnNames = expectedHeaderCells
            };

            target.Transform(document, transformer.Object);
            
            transformer.Verify(
                x => x.TransformHeader(
                    It.Is<string[]>(
                        arg => arg == expectedHeaderCells)));
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_InvokeAMethodOn_TransformationRunner_Once_InOrderToTransformDocumentRows(
            IList<string[]> rows)
        {
            Setup_HeaderTransformation(new string[] { "one", "two"});

            var expectedRows = rows.ToArray();

            Must_InvokeAMethodOn_TransformationRunner_Once_InOrderToTransformDocumentRows(expectedRows);
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_InvokeAMethodOn_TransformationRunner_Once_InOrderToTransformDocumentRows_EvenWhenThereAreNoColumsAfterHeaderTransformation(
            IList<string[]> rows)
        {
            Setup_HeaderTransformation(new string[] { "one", "two" });

            var expectedRows = rows.ToArray();

            Must_InvokeAMethodOn_TransformationRunner_Once_InOrderToTransformDocumentRows(expectedRows);
        }

        private void Must_InvokeAMethodOn_TransformationRunner_Once_InOrderToTransformDocumentRows(
            string[][] expectedRows)
        {
            var document = new CsvDocument
            {
                Rows = expectedRows,
                ColumnNames = new string[0]
            };

            target.Transform(document, transformer.Object);

            rowTransformationRunner.Verify(
                x => x.TransformRows(
                    It.IsAny<IStructureTransformer>(),
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
                    It.IsAny<IStructureTransformer>(),
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
                    It.Is<IStructureTransformer>(
                        arg => arg == transformer.Object),
                    It.IsAny<string[][]>(),
                    It.IsAny<int>()));
        }

        [TestCaseSource(nameof(Get_HeaderCells))]
        public void Must_PassThe_TheLengthOfTheHeader_AsTheExpectedRowLength(
            IList<string> headerCells)
        {
            var expectedRowLength = headerCells.Count;

            Setup_HeaderTransformation(headerCells.ToArray());            

            var document = new CsvDocument
            {
                Rows = new string[0][],
                ColumnNames = new string[0]
            };

            target.Transform(document, transformer.Object);

            rowTransformationRunner.Verify(
                x => x.TransformRows(
                    It.IsAny<IStructureTransformer>(),
                    It.IsAny<string[][]>(),
                    It.Is<int>(
                        arg => arg == expectedRowLength)));
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_ReturnTheTransformedRows(
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

        [TestCaseSource(nameof(Get_ColumnNames))]
        public void Must_Return_TheTransformedColumnsCollection(
            IList<string> columnNames)
        {
            var expectedColumns = columnNames.ToArray();

         //   Setup_TransformationRunner(new string[0][]);
            Setup_HeaderTransformation(columnNames.ToArray());

            var document = new CsvDocument
            {
                Rows = new string[0][],
                ColumnNames = new string[0]
            };

            var result = target.Transform(document, transformer.Object);

            Assert.AreEqual(expectedColumns, result.ColumnNames);
        }

        private void Setup_TransformationRunner(string[][] returnValue)
        {
            GetSetup_TransformationRunner()
                .Returns(returnValue);
        }

        private Moq.Language.Flow.ISetup<IRowTransformationRunner<IRowTransformer>, string[][]> GetSetup_TransformationRunner()
        {
            return rowTransformationRunner.Setup(
                x => x.TransformRows(
                    It.IsAny<IStructureTransformer>(),
                    It.IsAny<string[][]>(),
                    It.IsAny<int>()));
        }

        private void Setup_HeaderTransformation(string[] returnValue)
        {
            transformer.Setup(
                x => x.TransformHeader(
                    It.IsAny<string[]>()))
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

        private static IEnumerable<TestCaseData> Get_HeaderCells()
        {
            yield return new TestCaseData(
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "zwei" });

            yield return new TestCaseData(
                new List<string>(0));
        }

        private static IEnumerable<TestCaseData> Get_ColumnNames()
        {
            yield return new TestCaseData(
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" });
        }

        public class NonWelcomeInvocatioException : Exception
        {

        }
    }
}