using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation
{
    public interface IDocumentTransformer
    {
        IEnumerable<string[]> TransformRows(IEnumerable<string[]> rows);
    }
}