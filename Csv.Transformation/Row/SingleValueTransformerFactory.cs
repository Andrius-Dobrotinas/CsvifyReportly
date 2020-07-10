using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public class SingleValueTransformerFactory : IRowTransformerFactory<ISingleValueTransformer>
    {
        private readonly string targetColumnName;
        private readonly IValueTransformer valueTransformer;

        public SingleValueTransformerFactory(
            string targetColumnName,
            IValueTransformer valueTransformer)
        {
            this.targetColumnName = targetColumnName;
            this.valueTransformer = valueTransformer;
        }

        public ISingleValueTransformer Build(IDictionary<string, int> columnIndexes)
        {
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new SingleValueTransformer(targetColumnIndex, valueTransformer);
        }
    }
}