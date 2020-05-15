using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public class TolerantDateComparer : IDateComparer
    {
        private readonly int tolerance;

        public TolerantDateComparer(int tolerance)
        {
            this.tolerance = tolerance;
        }

        /// <summary>
        /// Compares dates allowing the second date to be up to a specified number of days later
        /// </summary>
        public bool AreDatesEqual(DateTime targetDate, DateTime date2)
        {
            return targetDate == date2
                || (targetDate <= date2 && targetDate.AddDays(tolerance) >= date2);
        }
    }
}
