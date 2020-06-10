using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Rewriters
{
    public class TargetCurrencyValueSelectorTests
    {
        TargetCurrencyValueSelector target;

        [TestCase("GBP", "GBP", "10")]
        [TestCase("USD", "usd", "5")]
        [TestCase("EUR", "eur", "25")]
        public void When_CurrencyIs_GBP_ShouldReturnTheOriginalValue_CaseInsensitiveComparison(
            string targetCurrency,
            string currency,
            string value)
        {
            target = new TargetCurrencyValueSelector(targetCurrency);

            var result = target.GetValue(currency, value);

            Assert.AreEqual(value, result);
        }

        [TestCase("GBP", "EUR", "10")]
        [TestCase("EUR", "USD", "15")]
        [TestCase("USD", "JPY", "5")]
        public void When_CurrencyIs_NotGBP_ShouldReturnNull_CaseInsensitiveComparison(
            string targetCurrency,
            string currency,
            string value)
        {
            target = new TargetCurrencyValueSelector(targetCurrency);

            var result = target.GetValue(currency, value);

            Assert.AreEqual(null, result);
        }
    }
}