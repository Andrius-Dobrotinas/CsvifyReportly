using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedSingleCellValueEvaluator : ISingleCellValueEvaluator
    {
        private readonly ISingleCellValueEvaluator singleCellValueEvaluator;

        public InvertedSingleCellValueEvaluator(ISingleCellValueEvaluator singleCellValueEvaluator)
        {
            this.singleCellValueEvaluator = singleCellValueEvaluator;
        }
        public bool IsMatch(string[] row)
        {
            return !singleCellValueEvaluator.IsMatch(row);
        }
    }
}