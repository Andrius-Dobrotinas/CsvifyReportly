using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    /// <summary>
    /// Returns false for values that match a specified value
    /// </summary>
    public class InvertedValueComparer : IValueComparer
    {
        private readonly IValueComparer valueComparer;

        public InvertedValueComparer(IValueComparer valueComparer)
        {
            this.valueComparer = valueComparer;
        }

        public bool IsMatch(string value)
        {
            return !valueComparer.IsMatch(value);
        }
    }
}