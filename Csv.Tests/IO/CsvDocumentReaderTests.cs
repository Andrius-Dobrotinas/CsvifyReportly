using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public class CsvDocumentReaderTests
    {
        CsvDocumentReader target;
        Mock<ICsvStreamReader> streamReader;

        [SetUp]
        public void Setup ()
        {
            streamReader = new Mock<ICsvStreamReader>();
            target = new CsvDocumentReader(streamReader.Object);

            Setup_StreamReader(new string[0][]);
        }

        [Test]
        public void Must_ReadTheRows_AsStrings()
        {
            var stream = new Mock<Stream>();
            
            target.Read(stream.Object);

            streamReader.Verify(
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

            for(int i = 0; i < expected.Length; i++)
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
            streamReader.Setup(
                x => x.Read(
                    It.IsAny<Stream>()))
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