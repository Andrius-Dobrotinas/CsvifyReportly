using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Runs a transformation on a given document using a given transformer
    /// </summary>
    public interface ITransformationRunner<IRowTransformer>
    {
        /// <param name="document">Source data</param>
        /// <param name="transformer">An component to use for the transformation</param>
        CsvDocument Transform(CsvDocument document, IRowTransformer transformer);
    }
}