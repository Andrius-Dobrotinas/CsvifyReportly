using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Transforms a given row using a given content transformer
    /// </summary>
    public interface IRowTransformationRunner<TRowTransformer>
        where TRowTransformer : IRowTransformer
    {
        string[][] TransformRows(TRowTransformer transformer, string[][] rows);
    }
}