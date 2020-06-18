using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Builds pre-configured instances of <see cref="TRowTransformer"/>
    /// </summary>
    /// <typeparam name="TRowTransformer">Type of transformation</typeparam>
    public interface IRowTransformerFactory<out TRowTransformer> : IDocumentTransformerFactory<TRowTransformer>
        where TRowTransformer : IRowTransformer
    {
    }
}