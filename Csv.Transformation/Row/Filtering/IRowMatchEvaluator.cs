using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public interface IRowMatchEvaluator
    {
        bool IsMatch(string[] row);
    }
}