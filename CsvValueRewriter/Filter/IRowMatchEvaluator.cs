using System;
using System.Collections.Generic;

namespace Andy.Csv.Rewrite.Filter
{
    public interface IRowMatchEvaluator
    {
        bool IsMatch(string[] row);
    }
}