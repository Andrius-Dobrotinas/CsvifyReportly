using System;
using System.Collections.Generic;

namespace Andy.Csv.Rewrite
{
    public interface ICsvRewriter
    {
        IEnumerable<string[]> Rewrite(IEnumerable<string[]> sourceRows);
    }
}