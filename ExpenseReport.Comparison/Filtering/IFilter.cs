using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Filtering
{
    public interface IFilter<TTransaction>
    {
        IEnumerable<TTransaction> FilterOut(IEnumerable<TTransaction> source);
    }
}