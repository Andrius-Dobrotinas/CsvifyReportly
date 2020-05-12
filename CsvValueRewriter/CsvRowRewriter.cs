using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Rewrite.Value
{
    public interface ICsvRowRewriter
    {
        IEnumerable<string[]> Rewrite(IEnumerable<string[]> source);
    }

    public class CsvRowRewriter : ICsvRowRewriter
    {
        private readonly int targetColumnIndex;
        private readonly IValueRewriter dateRewriter;

        public CsvRowRewriter(int targetColumnIndex, IValueRewriter dateRewriter)
        {
            this.targetColumnIndex = targetColumnIndex;
            this.dateRewriter = dateRewriter;
        }
        
        public IEnumerable<string[]> Rewrite(IEnumerable<string[]> source)
        {
            foreach (var item in source)
                Rewrite(item, targetColumnIndex);

            return source;
        }

        private void Rewrite(string[] row, int targetColumnIndex)
        {
            row[targetColumnIndex] = dateRewriter.Rewrite(row[targetColumnIndex]);
        }
    }
}