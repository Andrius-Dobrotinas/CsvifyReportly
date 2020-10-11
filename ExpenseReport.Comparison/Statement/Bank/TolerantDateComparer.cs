using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public class TolerantDateComparer : IDateComparer
    {
        private readonly int tolerance;
        private readonly DateComparisonDirection direction;

        public TolerantDateComparer(int tolerance, DateComparisonDirection direction)
        {
            this.tolerance = tolerance;
            this.direction = direction;
        }

        /// <summary>
        /// Compares dates allowing the second date to be up to a specified number of days later
        /// </summary>
        public bool AreDatesEqual(DateTime targetDate, DateTime date2)
        {
            if (targetDate == date2)
                return true;

            switch (direction)
            {
                case DateComparisonDirection.Up:
                    return CompareUp(targetDate, date2, tolerance);
                case DateComparisonDirection.Down:
                    return CompareDown(targetDate, date2, tolerance);
                default:
                    return false;
            }
        }

        public static bool CompareUp(DateTime targetDate, DateTime date2, int tolerance)
        {
            return targetDate < date2 && targetDate.AddDays(tolerance) >= date2;
        }

        public static bool CompareDown(DateTime targetDate, DateTime date2, int tolerance)
        {
            return targetDate > date2 && targetDate.AddDays(-tolerance) <= date2;
        }
    }
}
