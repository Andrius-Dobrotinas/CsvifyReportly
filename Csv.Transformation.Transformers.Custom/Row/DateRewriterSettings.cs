using System;

namespace Andy.Csv.Transformation.Row
{
    public class DateRewriterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }
        public string SourceFormat { get; set; }
        public string TargetFormat { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new SingleValueTransformerFactory(
                nameof(DateRewriterSettings),
                TargetColumnName,
                    new DateTransformer(SourceFormat, TargetFormat));
        }
    }
}