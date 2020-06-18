using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public interface IRowFilter
    {
        IEnumerable<string[]> Filter(IEnumerable<string[]> rows, IRowMatchEvaluator rowMatchEvaluator);
    }

    public class RowFilter : IRowFilter
    {
        public IEnumerable<string[]> Filter(IEnumerable<string[]> rows, IRowMatchEvaluator rowMatchEvaluator)
            => rows.Where(rowMatchEvaluator.IsMatch);
    }
}