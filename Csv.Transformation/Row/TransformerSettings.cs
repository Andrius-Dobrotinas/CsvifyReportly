using System;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Defines settings for a transformer, and builds instances of a factory for said transformer using these settings
    /// </summary>
    public abstract class TransformerSettings
    {
        public string _Description { get; set; }

        /// <summary>
        /// Builds an instance of a transformer factory using current settings
        /// </summary>
        public abstract IDocumentTransformerFactory BuildFactory();

        /// <summary>
        /// Returns either a user-defined description or, in case of the absence of one,
        /// a default name for the type
        /// </summary>
        protected string GetDescription() => string.IsNullOrEmpty(_Description)
            ? this.GetType().ToString()
            : _Description;
    }
}