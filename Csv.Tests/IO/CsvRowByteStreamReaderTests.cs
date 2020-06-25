using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.Csv.IO
{
    public class CsvRowByteStreamReaderTests
    {
        CsvRowByteStreamReader target;
        Mock<ICellByteStreamReader> rowParser;
        Mock<IStreamReaderFactory> streamReaderFactory;
        Mock<IStreamReaderPositionReporter> streamPositionReporter;
        Mock<Stream> stream;
        Mock<StreamReader> streamReader;

        [SetUp]
        public void Setup()
        {
            rowParser = new Mock<ICellByteStreamReader>();
            streamReaderFactory = new Mock<IStreamReaderFactory>();
            streamPositionReporter = new Mock<IStreamReaderPositionReporter>();
            target = new CsvRowByteStreamReader(
                rowParser.Object,
                streamReaderFactory.Object,
                streamPositionReporter.Object);

            stream = new Mock<Stream>();
            stream.Setup(x => x.CanRead).Returns(true);

            streamReader = new Mock<StreamReader>(stream.Object);
            Setup_StreamReaderFactory(streamReader.Object);
        }
        
        [TestCase(1)]
        [TestCase(3)]
        public void Must_ReadAllRows_UntilTheEndOfStream(int rowCount)
        {
            Setup_StreamReader_NumberOfRows(rowCount);            
            Setup_RowParser_ReturnSequence_ThenThrowAnException(GetFakeRows(rowCount));

            target.ReadRows(stream.Object)
                .ToArray();

            rowParser.Verify(
                x => x.ReadNextRow(
                    It.Is<StreamReader>(
                        arg => arg == streamReader.Object)),
                Times.Exactly(rowCount));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Must_ReadRows_OnlyWhenEnumerated(int rowCount)
        {
            Setup_StreamReader_NumberOfRows(rowCount);
            Setup_RowParser_ReturnSequence_ThenThrowAnException(GetFakeRows(rowCount));

            var result = target.ReadRows(stream.Object);

            rowParser.Verify(
                x => x.ReadNextRow(
                    It.IsAny<StreamReader>()),
                Times.Never,
                "Must not interact with the row parser until the result is enumerated");

            int currentIndex = 0;
            foreach(var item in result)
            {
                rowParser.Verify(
                    x => x.ReadNextRow(
                        It.Is<StreamReader>(
                            arg => arg == streamReader.Object)),
                    Times.Once,
                    $"Must read each line as requested (current item {currentIndex})");

                //clear the invocation list so i can verify that there's exactly one invocation each iteration
                rowParser.Invocations.Clear();
                currentIndex++;
            }
        }

        [TestCase(1)]
        [TestCase(3)]
        public void On_SubsequentEnumerations_Must_SimplyNotReturnAnything(int rowCount)
        {
            Setup_StreamReader_NumberOfRows(rowCount);
            Setup_RowParser_ReturnSequence_ThenThrowAnException(GetFakeRows(rowCount));

            var result = target.ReadRows(stream.Object);
            result.ToArray();

            Assert.IsEmpty(result.ToArray());
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void Must_ReturnAllRows(IList<string[]> rows)
        {
            var expectedRows = rows.ToArray();
            
            Setup_StreamReader_NumberOfRows(rows.Count);
            Setup_RowParser_ReturnSequence_ThenThrowAnException(rows);

            var result = target.ReadRows(stream.Object)
                .ToArray();

            AssertionExtensions.SequencesAreEqual(expectedRows, result);
        }

        [Test]
        public void When_TheSourceHasNoData__Must_ReturnAnEmptyCollection()
        {
            Setup_EndOfStream(true);

            var result = target.ReadRows(stream.Object)
                .ToArray();

            Assert.IsEmpty(result);
        }

        [TestCaseSource(nameof(Get_Rows))]
        public void When_TheRowReaderThrowsAnException_MustCatchItAndThrow_MyException(IList<string[]> rows)
        {
            Setup_StreamReader_NumberOfRows(rows.Count + 1);
            Setup_RowParser_ReturnSequence_ThenThrowAnException(rows);

            Assert.Throws<RowReadingException>(
                () => target.ReadRows(stream.Object).ToArray());
        }

        private void Setup_StreamReader_NumberOfRows(int rowCount)
        {
            var returnValueSequence = Enumerable
                .Repeat(false, rowCount)
                .Concat(Enumerable.Repeat(true, 10)); // just to simulate the behavior where all subsequent calls return true for End Of Stream

            Setup_EndOfStream(returnValueSequence);
        }

        private void Setup_EndOfStream(IEnumerable<bool> returnValues)
        {
            var setup = streamPositionReporter.SetupSequence(
                x => x.IsEndOfStream(
                    It.IsAny<StreamReader>()));

            foreach (var returnValue in returnValues)
                setup.Returns(returnValue);
        }

        private void Setup_EndOfStream(bool returnValue)
        {
            streamPositionReporter.Setup(
                x => x.IsEndOfStream(
                    It.IsAny<StreamReader>()))
                .Returns(returnValue);
        }

        private void Setup_StreamReaderFactory(StreamReader returnValue)
        {
            streamReaderFactory.Setup(
                x => x.Build(
                    It.IsAny<Stream>()))
                .Returns(returnValue);
        }

        private static IEnumerable<string[]> GetFakeRows(int rowCount)
        {
            var fakeRow = new[] { "string" };
            return Enumerable.Repeat(fakeRow, rowCount);
        }

        private void Setup_RowParser_ReturnSequence_ThenThrowAnException(IEnumerable<string[]> returnValues)
        {
            var setup = rowParser.SetupSequence(
                x => x.ReadNextRow(
                    It.IsAny<StreamReader>()));

            foreach(var returnValue in returnValues)
                setup.Returns(returnValue);

            setup.Throws<System.IO.EndOfStreamException>();
        }

        private static IEnumerable<TestCaseData> Get_Rows()
        {
            yield return new TestCaseData(
                new List<string[]> { 
                    new string[] { "one", "one-2" }
                });

            yield return new TestCaseData(
                new List<string[]> {
                    new string[] { "one", "one-2", "one-3" },
                    new string[] { "two", "two-2", "two-3" },
                    new string[] { "three", "three-2", "three-3" }
                });
        }
    }
}