using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public interface ISingleValueTransformer : ICellContentTransformer
    {
    }

    /// <summary>
    /// Transforms a value of a single column
    /// </summary>
    public class SingleValueTransformer : ISingleValueTransformer
    {
        private readonly int targetColumnIndex;
        private readonly IValueTransformer valueTransformer;

        public SingleValueTransformer(int targetColumnIndex, IValueTransformer valueTransformer)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.valueTransformer = valueTransformer;
        }

        public string[] Transform(string[] row)
        {
            row[targetColumnIndex] = valueTransformer.GetValue(row[targetColumnIndex]);

            return row;
        }
    }
}