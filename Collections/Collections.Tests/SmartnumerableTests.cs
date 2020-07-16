using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Collections
{
    public class SmartnumerableTests
    {
        Smartnumerable<string> target;

        [Test]
        public void GetEnumerator_Must_ReturnTheProvidedEnumerator()
        {
            var snumerator = new Smartnumerator<string>(
                new Mock<IEnumerator<string>>().Object);

            target = new Smartnumerable<string>(snumerator);

            var enumerator = target.GetEnumerator();

            Assert.AreEqual(snumerator, enumerator);
        }

        [TestCase(1)]
        [TestCase(3)]
        public void GetEnumerator_SubsequentInvocations_Must_ReturnTheSameProvidedEnumerator(
            int subsequentInvocationCount)
        {
            var snumerator = new Smartnumerator<string>(
                new Mock<IEnumerator<string>>().Object);

            target = new Smartnumerable<string>(snumerator);

            target.GetEnumerator();

            for (int i = 2; i < subsequentInvocationCount + 2; i++)
                Assert.AreEqual(snumerator, target.GetEnumerator(), $"invocation #{i}");
        }
    }
}