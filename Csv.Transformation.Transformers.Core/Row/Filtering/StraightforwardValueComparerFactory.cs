using Andy.Csv.Transformation.Comparison.String;
using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public static class StraightforwardValueComparerFactory
    {
        public static StraightforwardValueComparer Build(string targetValue, bool ignoreCase, ValueComparisonMethod method)
        {
            var mode = ignoreCase ?
                StringComparison.CurrentCultureIgnoreCase :
                StringComparison.CurrentCulture;

            var stringValueComparer = StringValueComparerResolver.Build(method, mode);

            return new StraightforwardValueComparer(targetValue, stringValueComparer);
        }
    }
}