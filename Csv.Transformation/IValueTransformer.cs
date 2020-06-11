using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation
{
    public interface IValueTransformer
    {
        string GetValue(string value);
    }
}