using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    public interface IMultiTransformer : IDocumentTransformer
    {
    }

    public class MultiTransformer : IMultiTransformer
    {
        private readonly IEnumerable<IDocumentTransformer> transformers;

        public MultiTransformer(IEnumerable<IDocumentTransformer> transformers)
        {
            this.transformers = transformers ?? throw new ArgumentNullException(nameof(transformers));
        }

        public CsvDocument Transform(CsvDocument document)
        {
            foreach (var rewriter in transformers)
                document = rewriter.Transform(document);

            return document;
        }
    }
}