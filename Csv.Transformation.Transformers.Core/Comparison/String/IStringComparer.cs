using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Comparison.String
{
    public interface IStringComparer
    {
        bool IsMatch(string targetValue, string value);
    }
}