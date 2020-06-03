using System;
using System.Collections.Generic;

namespace Andy.Csv.Rewrite
{
    public interface IValueRewriter
    {
        string Rewrite(string value);
    }
}