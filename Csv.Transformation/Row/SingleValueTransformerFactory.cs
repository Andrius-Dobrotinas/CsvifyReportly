using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public class SingleValueTransformerFactory : IRowTransformerFactory<SingleValueTransformer>
    {
        private readonly string targetColumnName;
        private readonly IValueTransformer dateRewriter;

        public SingleValueTransformerFactory(
            string targetColumnName,
            IValueTransformer dateRewriter)
        {
            this.targetColumnName = targetColumnName;
            this.dateRewriter = dateRewriter;
        }

        public SingleValueTransformer Build(IDictionary<string, int> columnIndexes)
        {
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new SingleValueTransformer(targetColumnIndex, dateRewriter);
        }
    }
}