using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public interface IMatchFinder<TItem1, TItem2>
    {
        IList<Tuple<TItem1, TItem2>> GetMatches(
            IList<TItem1> statement,
            IList<TItem2> transactions);
    }
}