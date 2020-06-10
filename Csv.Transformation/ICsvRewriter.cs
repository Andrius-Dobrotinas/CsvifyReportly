using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation
{
    public interface ICsvRewriter
    {
        IEnumerable<string[]> Rewrite(IEnumerable<string[]> sourceRows);
    }
}