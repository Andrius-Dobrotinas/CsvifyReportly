using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Comparison.String
{
    public class ContainsComparer : IStringComparer
    {
        private readonly StringComparison mode;

        public ContainsComparer(StringComparison mode)
        {
            this.mode = mode;
        }

        public bool IsMatch(string targetValue, string value)
        {
            return value.Contains(targetValue, mode);
        }
    }
}