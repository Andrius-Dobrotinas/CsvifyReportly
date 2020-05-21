using System;
using System.Collections.Generic;

namespace Andy.Csv.Rewrite.Value
{
    public interface IRowRewriter
    {
        string[] Rewrite(string[] row);
    }
}