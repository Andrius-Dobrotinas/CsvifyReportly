using Andy.Collections;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public class CsvReenumerableRowByteStreamReaderTests
    {
        CsvReenumerableRowByteStreamReader target;
        Mock<ICsvRowByteStreamReader> expensiveReader;

        [SetUp]
        public void Setup()
        {
            expensiveReader = new Mock<ICsvRowByteStreamReader>();

            target = new CsvReenumerableRowByteStreamReader(expensiveReader.Object);
        }
        
        [Test]
        public void Must_AskTheExpensiveReader_ToReadAllRows()
        {
            var input = new Mock<Stream>().Object;
            target.ReadRows(input);

            expensiveReader.Verify(
                x => x.ReadRows(
                    It.Is<Stream>(
                        arg => arg == input)));
        }

        [Test]
        public void Must_ReturnTheResultsAsSmartnumerable()
        {
            var rows = GetEnumerable();

            expensiveReader.Setup(
                x => x.ReadRows(
                    It.IsAny<Stream>()))
                .Returns(rows);

            var input = new Mock<Stream>().Object;
            var result = target.ReadRows(input);

            AssertionExtensions.SequencesAreEqual(rows, result);
            Assert.IsTrue(result is Smartnumerable<string[]>);
        }

        private IEnumerable<string[]> GetEnumerable()
        {
            yield return new string[0];
            yield return new string[] { "value" };
            yield return new string[] { "value", "value 2" };
        }
    }
}