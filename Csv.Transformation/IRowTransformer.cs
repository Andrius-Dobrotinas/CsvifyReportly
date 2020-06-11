using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation
{
    public interface IRowTransformer
    {
        string[] Tramsform(string[] row);
    }
}