using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    class TolerantDateComparerTests
    {
        TolerantDateComparer target;

        private void CreateTarget(int tolerance, DateComparisonDirection direction)
        {
            target = new TolerantDateComparer(tolerance, direction);
        }

        [TestCaseSource(nameof(Get_EqualDates))]
        public void When_No_VariationIsAllowed_And_Dates_AreExactlyTheSame__Should_Return_True(
            DateTime date1,
            DateTime date2)
        {
            CreateTarget(0, DateComparisonDirection.Both);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(Get_EqualDates))]
        public void When_VariationIsAllowed_Up_And_Dates_AreExactlyTheSame__Should_Return_True(
            DateTime date1,
            DateTime date2)
        {
            CreateTarget(5, DateComparisonDirection.Up);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(Get_EqualDates))]
        public void When_VariationIsAllowed_Down_And_Dates_AreExactlyTheSame__Should_Return_True(
            DateTime date1,
            DateTime date2)
        {
            CreateTarget(5, DateComparisonDirection.Down);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(Get_EqualDates))]
        public void When_VariationIsAllowed_EitherWay_And_Dates_AreExactlyTheSame__Should_Return_True(
            DateTime date1,
            DateTime date2)
        {
            CreateTarget(5, DateComparisonDirection.Both);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(Get_UnequalDates))]
        public void When_VariationIsNotAllowed_And_DatesAreNotEqual__Should_Return_False(
            DateTime date1,
            DateTime date2)
        {
            CreateTarget(0, DateComparisonDirection.Both);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_EqualDatesWithTolerance))]
        public void When_VariationIsAllowed_Up_And_TheSecondDateIsGreaterByUpToASpecifiedToleranceAmount__Should_Return_True(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Up);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(Get_EqualDatesWithTolerance))]
        public void When_VariationIsAllowed_Down_And_TheFirstDateIsGreaterByUpToASpecifiedToleranceAmount__Should_Return_True(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Down);

            var result = target.AreDatesEqual(date2, date1);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(Get_EqualDatesWithTolerance))]
        public void When_VariationIsAllowed_EitherWay_And_TheSecondDateIsGreaterByUpToASpecifiedToleranceAmount__Should_Return_True(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Both);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(Get_EqualDatesWithTolerance))]
        public void When_VariationIsAllowed_EitherWay_And_TheFirstDateIsGreaterByUpToASpecifiedToleranceAmount__Should_Return_True(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Both);

            var result = target.AreDatesEqual(date2, date1);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(Get_UnequalDatesWithTolerance))]
        public void When_VariationIsAllowed_Up_But_TheSecondDateIsGreaterByMoreThanASpecifiedToleranceAmount__Should_Return_False(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Up);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_UnequalDatesWithTolerance))]
        public void When_VariationIsAllowed_Down_But_TheFirstDateIsGreaterByMoreThanASpecifiedToleranceAmount__Should_Return_False(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Down);

            var result = target.AreDatesEqual(date2, date1);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_UnequalDatesWithTolerance))]
        public void When_VariationIsAllowed_EitherWay_But_TheSecondDateIsGreaterByMoreThanASpecifiedToleranceAmount__Should_Return_False(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Both);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_UnequalDatesWithTolerance))]
        public void When_VariationIsAllowed_EitherWay_But_TheFirstDateIsGreaterByMoreThanASpecifiedToleranceAmount__Should_Return_False(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Both);

            var result = target.AreDatesEqual(date2, date1);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_SecondDateLower))]
        public void When_VariationIsAllowed_Up_But_TheSecondDateIsLowerByAnyAmount__Should_Return_False(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Up);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_SecondDateLower))]
        public void When_VariationIsAllowed_Down_But_TheFirstDateIsLowerByAnyAmount__Should_Return_False(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Down);

            var result = target.AreDatesEqual(date2, date1);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_SecondDateLower))]
        public void When_VariationIsAllowed_EitherWay_But_TheSecondDateIsLowerByAnyAmount__Should_Return_False(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Both);

            var result = target.AreDatesEqual(date1, date2);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(Get_SecondDateLower))]
        public void When_VariationIsAllowed_EitherWay_But_TheFirstDateIsLowerByAnyAmount__Should_Return_False(
            DateTime date1,
            DateTime date2,
            int tolerance)
        {
            CreateTarget(tolerance, DateComparisonDirection.Both);

            var result = target.AreDatesEqual(date2, date1);

            Assert.IsFalse(result);
        }

        private static IEnumerable<TestCaseData> Get_EqualDates()
        {
            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 20));

            yield return new TestCaseData(
                new DateTime(2021, 12, 16, 1, 2, 3),
                new DateTime(2021, 12, 16, 1, 2, 3));
        }

        private static IEnumerable<TestCaseData> Get_EqualDatesWithTolerance()
        {
            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 20, 1, 1, 1),
                1);

            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 21),
                1);

            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 21),
                3);

            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 23),
                3);
        }

        private static IEnumerable<TestCaseData> Get_UnequalDatesWithTolerance()
        {
            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 21, 1, 1, 1),
                1);

            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 23),
                2);
        }

        private static IEnumerable<TestCaseData> Get_UnequalDates()
        {
            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 19, 1, 1, 1));

            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 20, 1, 1, 1));

            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 21));

            yield return new TestCaseData(
                new DateTime(2020, 01, 22),
                new DateTime(2020, 01, 21));
        }

        private static IEnumerable<TestCaseData> Get_SecondDateLower()
        {
            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 19, 23, 23, 59),
                1);

            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 19),
                2);

            yield return new TestCaseData(
                new DateTime(2020, 01, 20),
                new DateTime(2020, 01, 18),
                10);
        }
    }
}
