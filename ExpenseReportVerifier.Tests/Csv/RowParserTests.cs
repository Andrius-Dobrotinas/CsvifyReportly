using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Csv
{
    public class RowParserTests
    {
        [TestCaseSource(nameof(GetTestCases1))]
        public void Should_SplitTheStringUsingTheSpecifiedDelimiter(string input, char delimiter, IList<string> expectedResult)
        {
            var result = RowParser.Parse(input, delimiter);

            Assert.IsTrue(expectedResult.SequenceEqual(result));
        }

        [TestCaseSource(nameof(GetTestCases2))]
        public void Should_SplitTheStringUsingTheSpecifiedDelimiter_IgnoringDelimitersWithinQuotationMarks(
            string input,
            char delimiter,
            IList<string> expectedResult)
        {
            var result = RowParser.Parse(input, delimiter);

            Assert.IsTrue(expectedResult.SequenceEqual(result));
        }

        [TestCaseSource(nameof(GetTestCases3))]
        public void When_ClosingQuotationMarkIsNotFollowedByADelimiter__Should_ThrowAnException(
            string input,
            char delimiter)
        {
            Assert.Throws<UnexpectedTokenException>(
                () => RowParser.Parse(input, delimiter));
        }

        private static IEnumerable<TestCaseData> GetTestCases1()
        {
            yield return new TestCaseData(
                "one,two,three",
                ',',
                new List<string> { "one", "two", "three" });

            yield return new TestCaseData(
                "one,two,,four",
                ',',
                new List<string> { "one", "two", "", "four" });

            yield return new TestCaseData(
                "one,,,four",
                ',',
                new List<string> { "one", "", "", "four" });

            yield return new TestCaseData(
                "one,two,,",
                ',',
                new List<string> { "one", "two", "", "" });

            yield return new TestCaseData(
                ",two,three",
                ',',
                new List<string> { "", "two", "three" });
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

            //yield return new TestCaseData(
            //    @"""one,one"" two",
            //    ',')
            //    .SetDescription("The entry starts, but doesn't end right after the closing quotation mark");
        }
    }
}