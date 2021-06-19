using System;

namespace Andy.Csv.Transformation.Row
{
    public class DateRewriterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }
        public string SourceFormat { get; set; }
        public string TargetFormat { get; set; }

        public override IDocumentTransformerFactory BuildFactory(ICultureSettings globalSettings)
        {
            return new SingleValueTransformerFactory(
                this.GetDescription(),
                TargetColumnName,
                    new DateTransformer(SourceFormat, TargetFormat));
        }
    }
}