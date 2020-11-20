using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class DateComparer : IDateComparer
    {
        public bool AreDatesEqual(DateTime targetDate, DateTime date2)
        {
            return targetDate == date2;
        }
    }
}