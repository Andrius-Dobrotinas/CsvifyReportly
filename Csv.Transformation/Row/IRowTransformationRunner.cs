using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Transforms a given row using a given content transforme
    /// and makes sure each row ends up being of the expected length
    /// </summary>
    public interface IRowTransformationRunner<TRowTransformer>
        where TRowTransformer : IRowTransformer
    {
        string[][] TransformRows(TRowTransformer transformer, string[][] rows, int expectedCellCount);
    }
}