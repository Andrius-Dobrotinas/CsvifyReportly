using Andy.Csv.Transformation.Comparison.String;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class StraightforwardValueComparerTests
    {
        Mock<IStringComparer> comparer;

        [SetUp]
        public void Setup()
        {
            comparer = new Mock<IStringComparer>();
        }

        [TestCase(null, "two")]
        [TestCase(null, "")]
        public void When_OneOfValuesIsNull_TargetValueIsNull_ButTheOtherOneIsNot_ValuesAreNotEqual(string targetValue, string value)
        {
            var target = new StraightforwardValueComparer(targetValue, comparer.Object);

            var result = target.IsMatch(value);

            Assert.IsFalse(result);
        }

        [TestCase("one", null)]
        [TestCase("", null)]
        public void When_OneOfValuesIsNull_TargetValueIsNotNull_ButTheOtherOneIs_ValuesAreNotEqual(string targetValue, string value)
        {
            var target = new StraightforwardValueComparer(targetValue, comparer.Object);

            var result = target.IsMatch(value);

            Assert.IsFalse(result);
        }

        [Test]
        public void When_OneOfValuesIsNull_BothTargetAndOtherValuesAreNull_ValuesAreEqual()
        {
            var target = new StraightforwardValueComparer(null, comparer.Object);

            var result = target.IsMatch(null);

            Assert.IsTrue(result);
        }

        [TestCase("one", "two")]
        [TestCase("", "two")]
        [TestCase("one", "")]
        [TestCase("", "")]
        public void When_ValuesAreNotNull_MustUseTheProvidedComparer(string targetValue, string value)
        {
            var target = new StraightforwardValueComparer(targetValue, comparer.Object);

            var result = target.IsMatch(value);

            comparer.Verify(
                x => x.IsMatch(
                    It.Is<string>(
                        arg => arg == targetValue),
                    It.Is<string>(
                        arg => arg == value)),
                Times.Once);
        }
    }
}