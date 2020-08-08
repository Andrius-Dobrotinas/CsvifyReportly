using System;

namespace Andy.Csv.Transformation.Row
{
    public class AmountInverterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new SingleValueTransformerFactory(
                this.GetDescription(),
                TargetColumnName,
                    new AmountInverter());
        }
    }
}