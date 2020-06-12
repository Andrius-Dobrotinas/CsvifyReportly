using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class RowTransformer : IDocumentTransformer
    {
        private readonly IRowTransformer rowRewriter;

        public RowTransformer(IRowTransformer rowRewriter)
        {
            this.rowRewriter = rowRewriter;
        }
        
        public IEnumerable<string[]> TransformRows(IEnumerable<string[]> rows)
            => rows.Select(rowRewriter.Tramsform);
    }
}