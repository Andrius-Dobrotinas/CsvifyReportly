using System;

namespace Andy.Csv.Transformation.Row
{
    public class AmountInverterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }

        public override IDocumentTransformerFactory BuildFactory(ICultureSettings globalSettings)
        {
            return new SingleValueTransformerFactory(
                this.GetDescription(),
                TargetColumnName,
                    new AmountInverter(
                        GetFmtProvidr(globalSettings.NumberFormatCultureCode)));
        }

        private static IFormatProvider GetFmtProvidr(string cultureCode)
        {
            if (string.IsNullOrEmpty(cultureCode))
                return System.Globalization.CultureInfo.InvariantCulture.NumberFormat;

            {
                try
                {
                    return System.Globalization.CultureInfo.GetCultureInfo(cultureCode).NumberFormat;
                }
                catch (Exception e)
                {
                    throw new Exception($"There's problem with a culture code specified in {nameof(AmountInverterSettings)}", e);
                }
            }
        }
    }
}