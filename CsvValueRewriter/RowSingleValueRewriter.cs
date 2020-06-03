using System;
using System.Collections.Generic;

namespace Andy.Csv.Rewrite
{
    public class RowSingleValueRewriter : IRowRewriter
    {
        private readonly int targetColumnIndex;
        private readonly IValueRewriter dateRewriter;

        public RowSingleValueRewriter(int targetColumnIndex, IValueRewriter dateRewriter)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.dateRewriter = dateRewriter;
        }

        public string[] Rewrite(string[] row)
        {
            row[targetColumnIndex] = dateRewriter.Rewrite(row[targetColumnIndex]);

            return row;
        }
    }
}