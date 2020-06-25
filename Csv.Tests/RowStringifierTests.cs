using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv
{
    public class RowStringifierTests
    {
        RowStringifier target;
        Mock<ICellValueEncoder> encoder;

        [SetUp]
        public void Setup()
        {
            encoder = new Mock<ICellValueEncoder>();
            target = new RowStringifier(encoder.Object);

            encoder
                .Setup(
                    x => x.Encode(
                        It.IsAny<string>(),
                        It.IsAny<char>()))
                .Returns<string, char>(
                    (value, delimiter) => value);
        }

        [TestCaseSource(nameof(GetNonEmptyValues))]
        public void Must_EncodeEveryNonEmptyValue(IList<string> input)
        {
            var delimiter = '.';

            var result = target.Stringifififiify(input, delimiter);

            foreach(var item in input)
            {
                encoder.Verify(
                    x => x.Encode(
                        It.Is<string>(
                            arg => arg == item),
                        It.IsAny<char>()),
                    Times.Once,
                    "Must encode every value");

                encoder.Verify(
                    x => x.Encode(
                        It.Is<string>(
                            arg => arg == item),
                        It.Is<char>(
                            arg => arg == delimiter)),
                    Times.Once,
                    "Must pass on the delimiter as well");
            }

            encoder
                .Verify(
                    x => x.Encode(
                        It.IsAny<string>(),
                        It.IsAny<char>()),
                    Times.Exactly(input.Count),
                    "Must invoke the method the number of times there are encodable items in the collection");
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void Must_PutAllValuesIntoASingleStringSeparatedByASuppliedDelimiter(IList<string> input, char delimiter, string expectedResult)
        {
            var result = target.Stringifififiify(input, delimiter);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Must_ReturnAnEmptyStringForAnEmptyInputCollection()
        {
            var result = target.Stringifififiify(new string[0], ',');

            Assert.AreEqual("", result);
        }

        [TestCaseSource(nameof(GetInputWithEmptyValues))]
        public void Must_TreatEmptyValuesAsValidValues(IList<string> input, char delimiter, string expectedResult)
        {
            var result = target.Stringifififiify(input, delimiter);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCaseSource(nameof(GetInputWithNullValues))]
        public void Must_TreatNullsAsValidValues(IList<string> input, char delimiter, string expectedResult)
        {
            var result = target.Stringifififiify(input, delimiter);

            Assert.AreEqual(expectedResult, result);
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                ',',
                "one");

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" },
                ',',
                "one,two,three");
        }

        private static IEnumerable<TestCaseData> GetNonEmptyValues()
        {
            yield return new TestCaseData(
                new List<string> { "one" });

            yield return new TestCaseData(
                new List<string> { "one", "two", "three" });
        }

        private static IEnumerable<TestCaseData> GetInputWithEmptyValues()
        {
            yield return new TestCaseData(
                new List<string> { "one", "", "three" },
                ',',
                "one,,three");

            yield return new TestCaseData(
                new List<string> { "one", "", "", "four" },
                ',',
                "one,,,four");

            yield return new TestCaseData(
                new List<string> { "one", "" },
                ',',
                "one,");

            yield return new TestCaseData(
                new List<string> { "one", "", "", "" },
                ',',
                "one,,,");

            yield return new TestCaseData(
                new List<string> { "", "two" },
                ',',
                ",two");

            yield return new TestCaseData(
                new List<string> { "", "", "two" },
                ',',
                ",,two");

            yield return new TestCaseData(
                new List<string> { "", "two", "" },
                ',',
                ",two,");

            yield return new TestCaseData(
                new List<string> { "" },
                ',',
                "");

            yield return new TestCaseData(
                new List<string> { "", "" },
                ',',
                ",");
        }

        private static IEnumerable<TestCaseData> GetInputWithNullValues()
        {
            yield return new TestCaseData(
                new List<string> { "one", null, "three" },
                ',',
                "one,,three");

            yield return new TestCaseData(
                new List<string> { "one", null },
                ',',
                "one,");

            yield return new TestCaseData(
                new List<string> { null, "two" },
                ',',
                ",two");

            yield return new TestCaseData(
                new List<string> { null },
                ',',
                "");

            yield return new TestCaseData(
                new List<string> { null, null },
                ',',
                ",");
        }
    }
}