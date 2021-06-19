using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    public class AmountInverterTests
    {
        AmountInverter target;

        [SetUp]
        public void Setup()
        {
            target = new AmountInverter(System.Globalization.CultureInfo.InvariantCulture);
        }

        [TestCaseSource(nameof(Get_Values))]
        public void When_ValueIsOfDecimalType__MustInvertIt(
            string value,
            string expectedValue)
        {
            var result = target.GetValue(value);

            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void When_ValueIsNotOfDecimalType__Must_ThrowAnException()
        {
            var value = "text";

            Assert.Throws<FormatException>(
                () => target.GetValue(value));
        }

        private static IEnumerable<TestCaseData> Get_Values()
        {
            yield return new TestCaseData( "1.2", "-1.2");

            yield return new TestCaseData("-5.3", "5.3");

            yield return new TestCaseData("667.01", "-667.01");

            yield return new TestCaseData("6", "-6");

            yield return new TestCaseData("-34.778", "34.778");
        }
    }
}