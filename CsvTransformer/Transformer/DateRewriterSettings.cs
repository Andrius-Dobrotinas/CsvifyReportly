using System;

namespace Andy.Csv.Transformation.Row.Document.Cmd.Transformer
{
    public class DateRewriterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }
        public string SourceFormat { get; set; }
        public string TargetFormat { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new SingleValueTransformerFactory(
                TargetColumnName,
                    new DateTransformer(SourceFormat, TargetFormat));
        }
    }
}