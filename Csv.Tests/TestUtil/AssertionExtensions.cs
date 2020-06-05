using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy
{
    public static class AssertionExtensions
    {
        public static void SequencesAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var actualLength = actual.Count();
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.IsTrue(i < actualLength, "Actual collection doesn't contain an element at position {i}");

                Assert.AreEqual(expected.ElementAt(i), actual.ElementAt(i), $"Element at position {i}");
            }
        }
    }
}