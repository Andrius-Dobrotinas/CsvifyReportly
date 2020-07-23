using System;

namespace Andy.Csv.Transformation.Row.Document.Cmd.Transformer
{
    public class AmountInverterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new SingleValueTransformerFactory(
                TargetColumnName,
                    new AmountInverter());
        }
    }
}