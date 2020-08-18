using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Comparison.String
{
    public class StartsWithComparer : IStringComparer
    {
        private readonly StringComparison mode;

        public StartsWithComparer(StringComparison mode)
        {
            this.mode = mode;
        }

        public bool IsMatch(string targetValue, string value)
        {
            return value.StartsWith(targetValue, mode);
        }
    }
}