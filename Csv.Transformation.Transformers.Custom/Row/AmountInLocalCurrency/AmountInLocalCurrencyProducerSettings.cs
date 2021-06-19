using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public class AmountInLocalCurrencyProducerSettings : TransformerSettings
    {
        public string AmountColumnName { get; set; }
        public string CurrencyColumnName { get; set; }
        public string ResultAmountColumnName { get; set; }
        public string TargetCurrency { get; set; }

        public override IDocumentTransformerFactory BuildFactory(ICultureSettings globalSettings)
        {
            return new AmountInLocalCurrencyProducerFactory(
                this.GetDescription(),
                AmountColumnName,
                CurrencyColumnName,
                ResultAmountColumnName,
                new TargetCurrencyValueSelector(TargetCurrency));
        }
    }
}