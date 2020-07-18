using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Collections
{
    public class Smartnumerable_Create_Tests
    {
        public void Must_CreateAnInstanceOfSmartnumerableType(
            IEnumerable<string> source)
        {
            var snumerable = Smartnumerable.Create(source);

            Assert.IsAssignableFrom<Smartnumerable<object>>(snumerable);
        }

        [TestCaseSource(nameof(GetEnumerables))]
        public void Must_ReturnAnInstanceThatWrapsTheSuppliedIEnumerable(
            IEnumerable<object> source)
        {
            var snumerable = Smartnumerable.Create(source);

            AssertionExtensions.SequencesAreEqual(source, snumerable);
        }

        private static IEnumerable<TestCaseData> GetEnumerables()
        {
            yield return new TestCaseData(
                new List<string> { "initial" });

            yield return new TestCaseData(
                new List<string> { "initial", "initial too" });

            yield return new TestCaseData(
                new List<object> { 5, 6, 3, 2, 1 });

            yield return new TestCaseData(
                new List<object>
                {
                    new char[0],
                    new char[] { 'a', 'b' }
                });
        }
    }
}