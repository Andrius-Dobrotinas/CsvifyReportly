using System;

namespace Andy.Csv.Transformation.Row
{
    public class AmountInverterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new SingleValueTransformerFactory(
                nameof(AmountInverterSettings),
                TargetColumnName,
                    new AmountInverter());
        }
    }
}