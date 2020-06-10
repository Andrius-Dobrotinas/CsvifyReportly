using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation
{
    public interface IValueRewriter
    {
        string Rewrite(string value);
    }
}