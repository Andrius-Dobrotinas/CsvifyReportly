using System;
using System.Collections.Generic;

namespace Andy.Csv.Rewrite
{
    public interface IRowRewriter
    {
        string[] Rewrite(string[] row);
    }
}