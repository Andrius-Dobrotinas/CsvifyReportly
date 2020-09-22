using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Csv
{
    public interface IComparerFactory<TTransaction1, TTransaction2>
    {
        IComparer<TTransaction1, TTransaction2> Build(
            ICsvRowParser<TTransaction1> entry1Parser,
            ICsvRowParser<TTransaction2> entry2Parser);
    }

    public class ComparerFactory<TTransaction1, TTransaction2> : IComparerFactory<TTransaction1, TTransaction2>
        where TTransaction1 : IHaveSourceData
        where TTransaction2 : IHaveSourceData
    {
        private readonly ICollectionComparer<TTransaction1, TTransaction2> comparer;

        public ComparerFactory(
            ICollectionComparer<TTransaction1, TTransaction2> comparer)
        {
            this.comparer = comparer;
        }

        public IComparer<TTransaction1, TTransaction2> Build(
            ICsvRowParser<TTransaction1> entry1Parser,
            ICsvRowParser<TTransaction2> entry2Parser)
        {
            return new Comparer<TTransaction1, TTransaction2>(comparer, entry1Parser, entry2Parser);
        }
    }
}