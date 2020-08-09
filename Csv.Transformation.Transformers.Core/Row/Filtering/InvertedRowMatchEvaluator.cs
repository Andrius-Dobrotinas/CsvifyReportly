using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedRowMatchEvaluator : IRowMatchEvaluator
    {
        private readonly IRowMatchEvaluator rowMatchEvaluator;

        public InvertedRowMatchEvaluator(IRowMatchEvaluator rowMatchEvaluator)
        {
            this.rowMatchEvaluator = rowMatchEvaluator;
        }
        public bool IsMatch(string[] row)
        {
            return !rowMatchEvaluator.IsMatch(row);
        }
    }
}