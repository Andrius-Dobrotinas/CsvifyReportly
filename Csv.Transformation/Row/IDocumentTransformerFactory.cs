using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Builds pre-configured instances of <see cref="T"/>
    /// </summary>
    /// <typeparam name="T">Type of transformation</typeparam>
    public interface IDocumentTransformerFactory<out T>
    {
        T Build(IDictionary<string, int> columnNameIndexMap);
    }
}