using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public class SafeCsvRowByteStreamReaderTests
    {
        SafeCsvRowByteStreamReader target;
        Mock<ICsvRowByteStreamReader> csvReader;

        [SetUp]
        public void Setup()
        {
            csvReader = new Mock<ICsvRowByteStreamReader>();
            target = new SafeCsvRowByteStreamReader(csvReader.Object);
        }
        
        [Test]
        public void Must_ReadAllRows_FromTheSuppliedStream()
        {
            var stream = new Mock<Stream>().Object;

            target.Read(stream);

            csvReader.Verify(
                x => x.ReadRows(
                    It.Is<Stream>(
                        arg => arg == stream)));
        }

        [TestCaseSource(nameof(Get_BadRows))]
        public void Must_ThrowAnException__WhenNotAllRowsAreTheSameLength_AsTheFirstOne(IList<string[]> rows)
        {
            Setup_TheReader(rows);

            Assert.Throws<StructureException>(
                () => target.Read(new Mock<Stream>().Object).ToArray());
        }

        [TestCaseSource(nameof(GetGoodRows))]
        public void Must_SuccessfullyReturnAllRows__WhenTheyAreAllOfTheSameLength(IList<string[]> rows)
        {
            var expectedCollection = rows.ToArray();

            Setup_TheReader(rows);

            var result = target.Read(new Mock<Stream>().Object).ToArray();

            AssertionExtensions.SequencesAreEqual(expectedCollection, result);
        }

        [Test]
        public void When_ThereAreNoRows__Must_SuccessfullyReturn_AnEmptyCollection()
        {
            Setup_TheReader(new string[0][]);

            var result = target.Read(new Mock<Stream>().Object).ToArray();

            Assert.IsEmpty(result);
        }

        private void Setup_TheReader(IEnumerable<string[]> returnValues)
        {
            csvReader.Setup(
                x => x.ReadRows(
                    It.IsAny<Stream>()))
                .Returns(returnValues);
        }

        private static IEnumerable<TestCaseData> Get_BadRows()
        {
            yield return new TestCaseData(
                new List<string[]> { 
                    new string[] { "one" },
                    new string[] { "one", "one-2" }
                });

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one", "two" },
                    new string[] { "one" }
                });

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one", "two" },
                    new string[] { "one", "one-2" },
                    new string[] { "one" }
                });

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one", "two" },
                    new string[] { "one", "one-2" },
                    new string[] { "one" },
                    new string[] { "one", "two" },
                });

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { },
                    new string[] { "one" },
                });
        }

        private static IEnumerable<TestCaseData> GetGoodRows()
        {
            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one" },
                    new string[] { "one" }
                });

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one", "two" },
                    new string[] { "two", "three" },
                    new string[] { "four", "five" },
                });

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { },
                    new string[] { },
                    new string[] { },
                });
        }
    }
}