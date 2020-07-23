using System;

namespace Andy.Csv.Transformation.Row.Document.Setup
{
    /// <summary>
    /// Defines settings for a transformer, and builds instances of a factory for said transformer using these settings
    /// </summary>
    public abstract class TransformerSettings
    {
        /// <summary>
        /// Builds an instance of a transformer factory using current settings
        /// </summary>
        public abstract IDocumentTransformerFactory BuildFactory();
    }
}