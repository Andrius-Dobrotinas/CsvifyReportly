using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Transforms actual cell contents of a row, but never the structure (ie number or sequence of cells)
    /// </summary>
    public interface ICellContentTransformer : IRowTransformer
    {
    }
}