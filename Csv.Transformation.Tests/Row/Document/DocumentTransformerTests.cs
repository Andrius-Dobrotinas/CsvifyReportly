using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class DocumentTransformerTests
    {
        DocumentTransformer<IRowTransformer> target;
        Mock<IColumnMapBuilder> columnMapBuilder;
        Mock<IRowTransformerFactory<IRowTransformer>> factory;
        Mock<ITransformationRunner<IRowTransformer>> transformerRunner;
        Mock<IResultReporter> repoter;

        [SetUp]
        public void Setup()
        {
            columnMapBuilder = new Mock<IColumnMapBuilder>();
            factory = new Mock<IRowTransformerFactory<IRowTransformer>>();
            transformerRunner = new Mock<ITransformationRunner<IRowTransformer>>();
            repoter = new Mock<IResultReporter>();

            target = new RowTransformer<IRowTransformer>(
                columnMapBuilder.Object,
                factory.Object,
                transformerRunner.Object,
                repoter.Object);
        }

        [TestCaseSource(nameof(Get_ColumnSequences))]
        public void Must_GetColumnIndexes_ForTheCurrentStateOfTheDocument(
            IList<string> columnNames)
        {
            var expectedColumnNameSequence = columnNames.ToArray();

            var document = new CsvDocument
            {
                HeaderCells = expectedColumnNameSequence,
                ContentRows = new string[0][]
            };

            target.Transform(document);

            columnMapBuilder.Verify(
                x => x.GetColumnIndexMap(
                    It.Is<string[]>(
                        arg => arg == expectedColumnNameSequence)),
                Times.Once);
        }

        [TestCaseSource(nameof(Get_ColumnNameToIndexMaps))]
        public void Must_BuildARowTransformer_WithTheFreshlyBuilt_ColumnNameToIndexMap(
            IDictionary<string, int> columnMap)
        {
            Setup_ColumnIndexMapBuilder(columnMap);

            var document = new CsvDocument
            {
                HeaderCells = new string[0],
                ContentRows = new string[0][]
            };

            target.Transform(document);

            factory.Verify(
                x => x.Build(
                    It.Is<IDictionary<string, int>>(
                        arg => arg == columnMap)),
                Times.Once);
        }

        [TestCaseSource(nameof(Get_ColumnNameToIndexMaps))]
        public void Must_RunTheTransformation_UsingATransformationRunner_WithAGivenDocument_AndTheFreshlyBuiltTransformer(
            IDictionary<string, int> columnMap)
        {
            var document = new CsvDocument
            {
                HeaderCells = new string[0],
                ContentRows = new string[0][]
            };

            Setup_ColumnIndexMapBuilder(columnMap);

            IRowTransformer rowTransformer = new Mock<IRowTransformer>().Object;
            Setup_TransformerFactory(rowTransformer);

            target.Transform(document);

            transformerRunner.Verify(
                x => x.Transform(
                    It.Is<CsvDocument>(
                        arg => arg == document),
                    It.IsAny<IRowTransformer>()),
                Times.Once,
                "Must pass the original CSV document onto the transformation runner thing");

            transformerRunner.Verify(
                x => x.Transform(
                    It.Is<CsvDocument>(
                        arg => arg == document),
                    It.Is<IRowTransformer>(
                        arg => arg == rowTransformer)),
                Times.Once,
                "Must pass the brand new freshly-built CSV row transformer onto the transformation runner thing");
        }

        private void Setup_ColumnIndexMapBuilder(IDictionary<string, int> returnValue)
        {
            columnMapBuilder.Setup(
                x => x.GetColumnIndexMap(
                    It.IsAny<string[]>()))
                .Returns(returnValue);
        }

        private void Setup_TransformerFactory(IRowTransformer returnValue)
        {
            factory.Setup(
                x => x.Build(
                    It.IsAny<IDictionary<string, int>>()))
                .Returns(returnValue);
        }

        private static IEnumerable<TestCaseData> Get_ColumnSequences()
        {
            yield return new TestCaseData(
                new List<string> { "ein" });

            yield return new TestCaseData(
                new List<string> { "ein", "zwei", "drei" });
        }

        private static IEnumerable<TestCaseData> Get_ColumnNameToIndexMaps()
        {
            yield return new TestCaseData(
                new Dictionary<string, int> {
                    { "ein", 0 }
                });

            yield return new TestCaseData(
                new Dictionary<string, int> {
                    { "ein", 0 },
                    { "zwei", 1 },
                    { "drei", 2 },
                });
        }
    }
}