using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Filter
{
    public interface IRowMatchEvaluator
    {
        bool IsMatch(string[] row);
    }
}