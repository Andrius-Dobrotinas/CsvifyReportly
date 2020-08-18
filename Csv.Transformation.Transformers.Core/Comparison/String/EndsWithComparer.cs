using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Comparison.String
{
    public class EndsWithComparer : IStringComparer
    {
        private readonly StringComparison mode;

        public EndsWithComparer(StringComparison mode)
        {
            this.mode = mode;
        }

        public bool IsMatch(string targetValue, string value)
        {
            return value.EndsWith(targetValue, mode);
        }
    }
}