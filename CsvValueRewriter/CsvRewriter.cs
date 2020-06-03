using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Rewrite
{
    public class CsvRewriter : ICsvRewriter
    {
        private readonly IRowRewriter rowRewriter;

        public CsvRewriter(IRowRewriter rowRewriter)
        {
            this.rowRewriter = rowRewriter;
        }
        
        public IEnumerable<string[]> Rewrite(IEnumerable<string[]> sourceRows)
            => sourceRows.Select(rowRewriter.Rewrite);
    }
}