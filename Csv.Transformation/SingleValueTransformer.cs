using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation
{
    public class SingleValueTransformer : IRowTransformer
    {
        private readonly int targetColumnIndex;
        private readonly IValueTransformer dateRewriter;

        public SingleValueTransformer(int targetColumnIndex, IValueTransformer dateRewriter)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.dateRewriter = dateRewriter;
        }

        public string[] Tramsform(string[] row)
        {
            row[targetColumnIndex] = dateRewriter.GetValue(row[targetColumnIndex]);

            return row;
        }
    }
}