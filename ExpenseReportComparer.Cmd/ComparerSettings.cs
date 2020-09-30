using Andy.ExpenseReport.Comparison;
using Andy.ExpenseReport.Comparison.Csv.Statement;
using System;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    /// <summary>
    /// Defines settings for a transaction comparer, and builds instances of said comparer using these settings
    /// </summary>
    public abstract class ComparerSettings
    {
        public string _Description { get; set; }

        /// <summary>
        /// Builds an instance of a transaction comparer using current settings
        /// </summary>
        public abstract IItemComparer<
            StatementEntryWithSourceData,
            StatementEntryWithSourceData> BuildComparer();

        /// <summary>
        /// Returns either a user-defined description or, in case of the absence of one,
        /// a default name for the type
        /// </summary>
        protected string GetDescription() => string.IsNullOrEmpty(_Description)
            ? this.GetType().ToString()
            : _Description;
    }
}