using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation
{
    public interface IRowRewriter
    {
        string[] Rewrite(string[] row);
    }
}