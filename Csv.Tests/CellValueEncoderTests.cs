using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv
{
    public class CellValueEncoderTests
    {
        readonly CellValueEncoder target = new CellValueEncoder();

        [TestCase("some, value", ',', ExpectedResult = "\"some, value\"")]
        [TestCase("some value,", ',', ExpectedResult = "\"some value,\"")]
        [TestCase(",some value", ',', ExpectedResult = "\",some value\"")]
        [TestCase(",", ',', ExpectedResult = "\",\"")]
        [TestCase("some, value, too", ',', ExpectedResult = "\"some, value, too\"")]
        public string When_AValueContainsADelimiterCharacter__Must_WrapItInQuotationMarks(string input, char delimiter)
        {
            return target.Encode(input, delimiter);
        }

        [TestCase("\"")]
        [TestCase("some \"value,")]
        [TestCase("\",some value\"")]
        public void When_AValueContainsQuotationMarks__Must_ThrowAnException(string input)
        {
            Assert.Throws<NotImplementedException>(
                () => target.Encode(input, '.'));
        }
    }
}