using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation
{
    public class DocumentTransformer : IDocumentTransformer
    {
        private readonly IRowTransformer rowRewriter;

        public DocumentTransformer(IRowTransformer rowRewriter)
        {
            this.rowRewriter = rowRewriter;
        }
        
        public IEnumerable<string[]> TransformRows(IEnumerable<string[]> rows)
            => rows.Select(rowRewriter.Tramsform);
    }
}