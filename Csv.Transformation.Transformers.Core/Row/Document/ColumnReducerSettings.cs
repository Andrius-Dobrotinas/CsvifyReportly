using System;

namespace Andy.Csv.Transformation.Row.Document
{
    public class ColumnReducerSettings : TransformerSettings
    {
        public string[] TargetColumnNames { get; set; }

        public override IDocumentTransformerFactory BuildFactory(ICultureSettings globalSettings)
        {
            return new ColumnReducerFactory(
                nameof(ColumnReducerSettings),
                TargetColumnNames);
        }
    }
}