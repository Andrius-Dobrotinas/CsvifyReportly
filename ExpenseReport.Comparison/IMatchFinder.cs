using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public interface IMatchFinder<TTransaction1, TTransaction2>
    {
        IList<Tuple<TTransaction1, TTransaction2>> GetMatches(
            IEnumerable<TTransaction1> transactions1,
            IEnumerable<TTransaction2> transactions2);
    }
}