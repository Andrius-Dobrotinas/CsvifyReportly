using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Comparison.String
{
    public class StraightComparer : IStringComparer
    {
        private readonly StringComparison mode;

        public StraightComparer(StringComparison mode)
        {
            this.mode = mode;
        }

        public bool IsMatch(string targetValue, string value)
        {
            return targetValue.Equals(value, mode);
        }
    }
}