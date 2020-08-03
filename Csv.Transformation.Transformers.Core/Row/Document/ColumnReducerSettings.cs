using System;

namespace Andy.Csv.Transformation.Row.Document
{
    public class ColumnReducerSettings : TransformerSettings
    {
        public string[] TargetColumnNames { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new ColumnReducerFactory(TargetColumnNames);
        }
    }
}