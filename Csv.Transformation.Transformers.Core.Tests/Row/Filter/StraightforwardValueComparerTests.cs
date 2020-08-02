using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class StraightforwardValueComparerTests
    {
        [TestCase("one", "one")]
        [TestCase("Okay, partner, let's keep on rolling", "Okay, partner, let's keep on rolling")]
        [TestCase("", "")]
        [TestCase(null, null)]
        public void Should_Return_False_ForValuesThatDontMatchTheTargetValue(string targetValue, string value)
        {
            var target = new StraightforwardValueComparer(targetValue);

            var result = target.IsMatch(value);

            Assert.IsFalse(result);
        }

        [TestCase("one", "One")]
        [TestCase("one", "")]
        [TestCase("one", null)]
        public void Should_Return_True_ForValuesThatDoMatchTheTargetValue(string targetValue, string value)
        {
            var target = new StraightforwardValueComparer(targetValue);

            var result = target.IsMatch(value);

            Assert.IsTrue(result);
        }

        [TestCase("", null)]
        [TestCase(null, "")]
        public void Should_Treat_NullsAndEmptyStrings_AsDifferentThings(string targetValue, string value)
        {
            var target = new StraightforwardValueComparer(targetValue);

            var result = target.IsMatch(value);

            Assert.IsTrue(result);
        }
    }
}