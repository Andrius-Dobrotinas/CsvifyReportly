using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.Statement
{
    public interface IHaveDate
    {
        public DateTime Date { get; }
    }
    public class OrderedCollectionComparer<TTransaction1, TTransaction2> : ICollectionComparer<TTransaction1, TTransaction2>
        where TTransaction1 : IHaveDate
        where TTransaction2 : IHaveDate
    {
        private readonly ICollectionComparer<TTransaction1, TTransaction2> comparinator;

        public OrderedCollectionComparer(ICollectionComparer<TTransaction1, TTransaction2> comparinator)
        {
            this.comparinator = comparinator;
        }

        public ComparisonResult<TTransaction1, TTransaction2> Compare(IList<TTransaction1> transactions1, IList<TTransaction2> transactions2)
        {
            return comparinator.Compare(
                transactions1.OrderBy(x => x.Date).ToArray(),
                transactions2.OrderBy(x => x.Date).ToArray());
        }
    }
}