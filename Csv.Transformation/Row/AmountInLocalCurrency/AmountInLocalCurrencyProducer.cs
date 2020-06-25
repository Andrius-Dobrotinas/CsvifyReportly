using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    public class AmountInLocalCurrencyProducer : ICellContentTransformer
    {
        private readonly int amountColumnIndex;
        private readonly int currencyColumnIndex;
        private readonly int targetColumnIndex;

        private readonly ITargetCurrencyValueSelector valueSelector;

        public AmountInLocalCurrencyProducer(
            int amountColumnIndex,
            int currencyColumnIndex,
            int targetColumnIndex,
            ITargetCurrencyValueSelector valueSelector)
        {
            this.amountColumnIndex = amountColumnIndex;
            this.currencyColumnIndex = currencyColumnIndex;
            this.targetColumnIndex = targetColumnIndex;
            this.valueSelector = valueSelector;
        }

        public string[] Tramsform(string[] source)
        {
            int lastIndex = source.Length - 1;

            if (targetColumnIndex > lastIndex)
                throw new Exception($"The row is too short to write a value at a target position (row length: {source.Length}, target index: {targetColumnIndex}");

            var newValue = valueSelector.GetValue(source[currencyColumnIndex], source[amountColumnIndex]);

            source[targetColumnIndex] = newValue;

            return source;
        }
    }
}