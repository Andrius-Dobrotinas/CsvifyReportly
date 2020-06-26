using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Serialization
{
    public class RowParserTests
    {
        [TestCaseSource(nameof(GetTestCases1))]
        public void Should_SplitTheStringUsingASpecifiedDelimiter(string input, char delimiter, IList<string> expectedResult)
        {
            var result = RowParser.Parse(input, delimiter);

            SequencesAreEqual(expectedResult, result);
        }

        [TestCaseSource(nameof(GetTestCases_WithEmptyEntries))]
        public void When_ADelimiterIsFollowedByADelimiter__Should_TreatThatAsAnEmptyEntry(string input, char delimiter, IList<string> expectedResult)
        {
            var result = RowParser.Parse(input, delimiter);

            SequencesAreEqual(expectedResult, result);
        }

        [TestCaseSource(nameof(GetTestCases_EndingWithADelimiter))]
        public void When_AStringEndsWithADelimiter__Should_AddAnEmptyEntryAtTheEndOfArray(string input, char delimiter, IList<string> expectedResult)
        {
            var result = RowParser.Parse(input, delimiter);

            SequencesAreEqual(expectedResult, result);
        }

        [TestCaseSource(nameof(GetTestCases2))]
        public void Should_SplitTheStringUsingTheSpecifiedDelimiter_IgnoringDelimitersWithinQuotationMarks(
            string input,
            char delimiter,
            IList<string> expectedResult)
        {
            var result = RowParser.Parse(input, delimiter);

            SequencesAreEqual(expectedResult, result);
        }

        [TestCaseSource(nameof(GetTestCases3))]
        public void When_ClosingQuotationMarkIsNotFollowedByADelimiter__Should_ThrowAnException(
            string input,
            char delimiter)
        {
            Assert.Throws<UnexpectedTokenException>(
                () => RowParser.Parse(input, delimiter));
        }

        [TestCaseSource(nameof(GetTestCasesWithQuoatationMarks))]
        public void When_EntryIsWrappedInQuotationMarks__Should_RemoveThem(
            string input,
            IList<string> expectedValues,
            char delimiter)
        {
            var result = RowParser.Parse(input, delimiter);

            foreach (var expected in expectedValues)
            {
                Assert.Contains(expected, result);
            }
        }

        private static void SequencesAreEqual(IList<string> expectedResult, IList<string> result)
        {
            var minLength = result.Count > expectedResult.Count ? expectedResult.Count : result.Count;
            for (int i = 0; i < minLength; i++)
            {
                Assert.AreEqual(expectedResult[i], result[i], $"Item at position {i}");
            }
            Assert.AreEqual(expectedResult.Count, result.Count);
        }

        private static IEnumerable<TestCaseData> GetTestCases1()
        {
            yield return new TestCaseData(
                "one,two",
                ',',
                new List<string> { "one", "two" });

            yield return new TestCaseData(
                "one,two,three",
                ',',
                new List<string> { "one", "two", "three" });

            yield return new TestCaseData(
                ",two,three",
                ',',
                new List<string> { "", "two", "three" });

            yield return new TestCaseData(
                "one,",
                ',',
                new List<string> { "one", "" });

            yield return new TestCaseData(
                ",",
                ',',
                new List<string> { "", "" });
        }

        private static IEnumerable<TestCaseData> GetTestCases_WithEmptyEntries()
        {
            yield return new TestCaseData(
                "one,,two",
                ',',
                new List<string> { "one", "", "two" });

            yield return new TestCaseData(
                "one,,,two",
                ',',
                new List<string> { "one", "", "", "two" });

            yield return new TestCaseData(
                "one,,,,four",
                ',',
                new List<string> { "one", "", "", "", "four" });

            yield return new TestCaseData(
               ",,,four",
               ',',
               new List<string> { "", "", "", "four" });
        }

        private static IEnumerable<TestCaseData> GetTestCases_EndingWithADelimiter()
        {
            yield return new TestCaseData(
                "one,",
                ',',
                new List<string> { "one", "" });

            yield return new TestCaseData(
                "one,two,",
                ',',
                new List<string> { "one", "two", "" });

            yield return new TestCaseData(
               ",",
               ',',
               new List<string> { "", "" });
        }

        private static IEnumerable<TestCaseData> GetTestCases2()
        {
            yield return new TestCaseData(
                @"""one,one""",
                ',',
                new List<string> { "one,one" });

            yield return new TestCaseData(
                @"""one,one"",two",
                ',',
                new List<string> { "one,one", "two" });

            yield return new TestCaseData(
                @"one,"",one"",two",
                ',',
                new List<string> { "one", ",one", "two" });

            yield return new TestCaseData(
                @"one,"","",two",
                ',',
                new List<string> { "one", ",", "two" });

            yield return new TestCaseData(
                @"one,"""",two",
                ',',
                new List<string> { "one", "", "two" });
        }

        private static IEnumerable<TestCaseData> GetTestCases3()
        {
            yield return new TestCaseData(
                @"""one,one"" ,two",
                ',')
                .SetDescription("The entry starts, but doesn't end right after the closing quotation mark");

            yield return new TestCaseData(
                @"""one,one""two",
                ',')
                .SetDescription("The separator is missing");

            yield return new TestCaseData(
                @"""one,one"" two",
                ',')
                .SetDescription("The separator is missing, there's a space char instead");
        }

        private static IEnumerable<TestCaseData> GetTestCasesWithQuoatationMarks()
        {
            yield return new TestCaseData(
                @"""one""",
                new List<string> { "one" },
                ',');

            yield return new TestCaseData(
                @"""one"",two",
                new List<string> { "one", "two" },
                ',');

            yield return new TestCaseData(
                @"""one,one"",two",
                new List<string> { "one,one", "two" },
                ',');
        }
    }
}