using Andy.Csv.Transformation.Comparison.String;
using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class StraightforwardValueComparer : IValueComparer
    {
        private readonly string targetValue;
        private readonly IStringComparer comparer;

        public StraightforwardValueComparer(string targetValue, IStringComparer comparer)
        {
            this.targetValue = targetValue;
            this.comparer = comparer;
        }

        public bool IsMatch(string value)
        {
            if (value == null)
                return targetValue == null;

            if (targetValue == null)
                return false;

            return comparer.IsMatch(targetValue, value);
        }
    }
}