using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public interface IRowTransformer
    {
        string[] Transform(string[] row);
    }
}