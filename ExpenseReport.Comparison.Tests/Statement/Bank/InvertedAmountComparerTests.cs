using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    class InvertedAmountComparerTests
    {
        InvertedAmountComparer target = new InvertedAmountComparer();

        [TestCase(1, 1)]
        [TestCase(-5, -5)]
        public void Must_Return_False__When_Amounts_AreTheSame(decimal one, decimal two)
        {
            var result = target.AreEqual(one, two);
            Assert.False(result);
        }

        [TestCase(1, -1)]
        [TestCase(-5, 5)]
        public void Must_Return_True__When_Amounts_AreTheOppositeOfEachOther(decimal one, decimal two)
        {
            var result = target.AreEqual(one, two);
            Assert.True(result);
        }

        public void Must_Return_True__When_Amounts_AreZero()
        {
            var result = target.AreEqual(0, 0);
            Assert.True(result);
        }
    }
}