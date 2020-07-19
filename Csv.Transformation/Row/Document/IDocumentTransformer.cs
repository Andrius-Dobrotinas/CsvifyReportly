using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Performs any sort of transformations on a CSV document
    /// </summary>
    public interface IDocumentTransformer
    {
        CsvDocument Transform(CsvDocument document);
    }
}