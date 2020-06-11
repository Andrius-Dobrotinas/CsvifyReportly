using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    public interface IDocumentTransformer
    {
        IEnumerable<string[]> TransformRows(IEnumerable<string[]> rows);
    }
}