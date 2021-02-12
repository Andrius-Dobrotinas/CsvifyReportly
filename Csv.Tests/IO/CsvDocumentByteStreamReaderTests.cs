using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public class CsvDocumentByteStreamReaderTests
    {
        delegate void HasDuplicatesDelegate(IEnumerable<string> input, out string[] output);

        CsvDocumentByteStreamReader target;
        Mock<IRowLengthValidatingCsvRowByteStreamReader> streamParser;
        Mock<IArrayValueUniquenessChecker> arrayValueUniquenessChecker;

        [SetUp]
        public void Setup()
        {
            streamParser = new Mock<IRowLengthValidatingCsvRowByteStreamReader>();
            arrayValueUniquenessChecker = new Mock<IArrayValueUniquenessChecker>();
            target = new CsvDocumentByteStreamReader(
                streamParser.Object,
                arrayValueUniquenessChecker.Object);

            Setup_StreamReader(new string[0][]);
        }

        [Test]
        public void Must_ReadTheRows_AsStrings()
        {
            var stream = new Mock<Stream>();

            target.Read(stream.Object);

            streamParser.Verify(
                x => x.Read(
                    It.Is<Stream>(
                        arg => arg == stream.Object)));
        }

        [Test]
        public void When_TheAreNoLines__Must_Return_Null()
        {
            Setup_StreamReader(new string[0][]);

            var stream = new Mock<Stream>();

            var result = target.Read(stream.Object);

            Assert.IsNull(result);
        }

        [TestCaseSource(nameof(Get_Content))]
        public void Must_Check_WhetherColumnsHaveUniqueNames(
            IList<string> headerCells,
            IList<string[]> contentRows)
        {
            var input = Combine(headerCells, contentRows);
            Setup_StreamReader(input);

            var stream = new Mock<Stream>();

            target.Read(stream.Object);

            arrayValueUniquenessChecker.Verify(
                x => x.HasDuplicates(
                    It.Is<string[]>(
                        arg => arg.SequenceEqual(headerCells)),
                    out It.Ref<string[]>.IsAny));
        }

        [Test]
        public void When_ColumnNamesAreNotUnique_Must_ThrowAnException()
        {
            Setup_StreamReader(new string[][] {
                new string[] {"colum name" },
                new string[] {"data cell" }
            });

            var stream = new Mock<Stream>();

            Setup_ColumnsHaveNonUniqueNames(true, new string[] { "" });

            Assert.Throws<StructureException>(
                () => target.Read(stream.Object));
        }

        [TestCaseSource(nameof(Get_Content))]
        public void Must_Return_FirstLineAsHeaderCells(
            IList<string> headerCells,
            IList<string[]> contentRows)
        {
            var expected = headerCells;

            var input = Combine(headerCells, contentRows);
            Setup_StreamReader(input);

            var stream = new Mock<Stream>();

            var result = target.Read(stream.Object);

            AssertionExtensions.SequencesAreEqual(expected, result.HeaderCells);
        }

        [TestCaseSource(nameof(Get_Content))]
        public void Must_Return_TheRestAsContentRows(
            IList<string> headerCells,
            IList<string[]> contentRows)
        {
            var expected = contentRows.ToArray();

            var input = Combine(headerCells, contentRows);
            Setup_StreamReader(input);

            var stream = new Mock<Stream>();

            var result = target.Read(stream.Object);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreNotEqual(i, result.ContentRows.Length, "The resulting collection can't be smaller than the expected one");

                AssertionExtensions.SequencesAreEqual(expected[i], result.ContentRows[i]);
            }
        }

        private static string[][] Combine(IList<string> headerCells, IList<string[]> contentRows)
        {
            return new string[][] { headerCells.ToArray() }
                .Concat(contentRows)
                .ToArray();
        }

        private void Setup_StreamReader(string[][] returnValue)
        {
            streamParser.Setup(
                x => x.Read(
                    It.IsAny<Stream>()))
                .Returns(returnValue);
        }

        private void Setup_ColumnsHaveNonUniqueNames(bool returnValue, string[] nonUniqueColumnNamesReturnValue)
        {
            var callbackAction = new HasDuplicatesDelegate(
                (IEnumerable<string> input, out string[] output) =>
                {
                    output = nonUniqueColumnNamesReturnValue;
                });

            arrayValueUniquenessChecker.Setup(
                x => x.HasDuplicates(
                    It.IsAny<IEnumerable<string>>(),
                    out It.Ref<string[]>.IsAny))
                .Callback(callbackAction)
                .Returns(returnValue);
        }

        private static IEnumerable<TestCaseData> Get_Content()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                new List<string[]> { });

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                new List<string[]> { });

            yield return new TestCaseData(
                new List<string> { "one" },
                new List<string[]> {
                    new string[] {"row1" }
                });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                new List<string[]> {
                    new string[] {"row1", "row1-1" },
                    new string[] {"row2", "row2-2" },
                });
        }
    }
}