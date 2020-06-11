using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation
{
    public interface ITargetCurrencyValueSelector
    {
        /// <summary>
        /// Returns a value if it's in target frequency. Otherwise returns a null.
        /// </summary>
        string GetValue(string currency, string amount);
    }

    public class TargetCurrencyValueSelector : ITargetCurrencyValueSelector
    {
        private readonly string targetCurrency;

        public TargetCurrencyValueSelector(string targetCurrency)
        {
            this.targetCurrency = targetCurrency;
        }

        public string GetValue(string currency, string amount)
        {
            if (currency.Equals(targetCurrency, StringComparison.InvariantCultureIgnoreCase))
                return amount;
            else
                return null;
        }
    }
}