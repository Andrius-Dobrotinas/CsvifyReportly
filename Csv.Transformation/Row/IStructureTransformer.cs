using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Modifies the structure of a CSV document by adding/removing and/or moving columns around
    /// </summary>
    public interface IStructureTransformer : IRowTransformer
    {
    }
}