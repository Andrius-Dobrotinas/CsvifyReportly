using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    // <summary>
    /// Produces an "Amount In Local Currency" value based on Amount and Currency values of a transaction
    /// </summary>
    public class CurrencyAmount_CantThinkOfName : IRowTransformer
    {
        private readonly int amountColumnIndex;
        private readonly int currencyColumnIndex;
        private readonly int targetColumnIndex;

        private readonly ITargetCurrencyValueSelector valueSelector;

        public CurrencyAmount_CantThinkOfName(
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