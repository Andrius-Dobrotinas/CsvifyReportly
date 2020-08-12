using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Comparison.String
{
    public enum ValueComparisonMethod
    {
        OneToOneMatch = 0,
        StartsWith = 1,
        Contains = 2,
        EndsWith = 3
    }
}