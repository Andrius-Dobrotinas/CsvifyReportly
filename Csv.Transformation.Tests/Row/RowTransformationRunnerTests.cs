using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    public class RowTransformationRunnerTests
    {
        RowTransformationRunner target = new RowTransformationRunner();
        Mock<ICellContentTransformer> transformer;

        [SetUp]
        public void Setup()
        {
            transformer = new Mock<ICellContentTransformer>();
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void Must_TransformEveryRow(
            IList<string[]> rows)
        {
            Setup_Transformer_ToReturnTheInput();

            target.TransformRows(transformer.Object, rows.ToArray(), rows[0].Length);

            for (int i = 0; i < rows.Count; i++)
            {
                transformer.Verify(
                    x => x.Transform(
                        It.Is<string[]>(
                            arg => arg == rows[i])),
                    Times.Once,
                    $"Must transform each row (current index {i})");
            }
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void Must_ReturnAllTransformedRows(
            IList<string[]> rows)
        {
            Setup_Transformer_ToReturnTheInput();

            var result = target.TransformRows(transformer.Object, rows.ToArray(), rows[0].Length);

            AssertionExtensions.SequencesAreEqual(rows, result);
        }

        [TestCaseSource(nameof(GetTestCases_LengthChanged))]
        public void When_TransformedRowsAreOfDifferentLengthThanExpected__Must_ThrowAnException(
            IList<string[]> rows,
            IList<string[]> transformedRows,
            int expectedRowCount)
        {
            Setup_TransformerSequence(transformedRows);

            Assert.Throws<CellCountMismatchException>(
                () => target.TransformRows(transformer.Object, rows.ToArray(), expectedRowCount));
        }

        [Test]
        public void When_SourceRowsCollectionIsEmpty__Must_ReturnTheOriginalValue()
        {
            var result = target.TransformRows(transformer.Object, new string[0][], 1);

            Assert.IsEmpty(result);
        }

        private void Setup_TransformerSequence(IList<string[]> returnValueSequence)
        {
            var sequence = transformer.SetupSequence(
                    x => x.Transform(
                        It.IsAny<string[]>()));

            foreach (var item in returnValueSequence)
            {
                sequence = sequence.Returns(item);
            }
        }

        private void Setup_Transformer_ToReturnTheInput()
        {
            transformer.Setup(
                    x => x.Transform(
                        It.IsAny<string[]>()))
                    .Returns<string[]>(row => row);
        }

        private static IEnumerable<TestCaseData> GetTestCases()
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

        private static IEnumerable<TestCaseData> GetTestCases_LengthChanged()
        {
            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one" }
                },
                new List<string[]> {
                    new string[] { "one", "two" }
                },
                1);

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one" }
                },
                new List<string[]> {
                    new string[] { }
                },
                1);

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one", "one two" },
                    new string[] { "two", "two two", "two three" },
                    new string[] { "three" }
                },
                new List<string[]> {
                    new string[] { "one", "two" },
                    new string[] { "two", "two three" },
                    new string[] { "three" }
                },
                2);
        }
    }
}