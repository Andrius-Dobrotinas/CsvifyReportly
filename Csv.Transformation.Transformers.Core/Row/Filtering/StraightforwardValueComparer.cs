using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class StraightforwardValueComparer : IValueComparer
    {
        private readonly string targetValue;
        private readonly StringComparison stringComparisonType;

        public StraightforwardValueComparer(string targetValue, bool isCaseInsensitive)
        {
            this.targetValue = targetValue;
            this.stringComparisonType = isCaseInsensitive ?
                StringComparison.CurrentCultureIgnoreCase :
                StringComparison.CurrentCulture;
        }

        public bool IsMatch(string value)
        {
            return targetValue.Equals(value, stringComparisonType);
        }
    }
}