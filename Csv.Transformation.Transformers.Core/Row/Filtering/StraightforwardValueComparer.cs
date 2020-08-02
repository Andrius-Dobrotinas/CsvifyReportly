using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class StraightforwardValueComparer : IValueComparer
    {
        private readonly string targetValue;

        public StraightforwardValueComparer(string targetValue)
        {
            this.targetValue = targetValue;
        }

        public bool IsMatch(string value)
        {
            return value != targetValue;
        }
    }
}