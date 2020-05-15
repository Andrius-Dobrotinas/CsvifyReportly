using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public interface IDateComparer
    {
        bool AreDatesEqual(DateTime date1, DateTime date2);
    }
}
