using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public interface IMatchFinder<TTransaction1, TTransaction2>
    {
        IList<Tuple<TTransaction1, TTransaction2>> GetMatches(
            IList<TTransaction1> transactions1,
            IList<TTransaction2> transactions2);
    }
}