using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public interface IValueComparer
    {
        bool IsMatch(string value);
    }
}