using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Rewrite.Rewriters
{
    /// <summary>
    /// Produces an "Amount In Local Currency" value based on Amount and Currency values of a transaction
    /// </summary>
    public class CurrencyAmount_CantThinkOfName : IRowRewriter
    {
        private readonly int amountColumnIndex;
        private readonly int currencyColumnIndex;
        private readonly int targetColumnIndex;

        private readonly ITargetCurrencyValueSelector gbpValueSelector;
        private readonly IArrayElementInserter<string> elementInserter;

        public CurrencyAmount_CantThinkOfName(
            int amountColumnIndex,
            int currencyColumnIndex,
            int targetColumnIndex,
            ITargetCurrencyValueSelector gbpValueSelector,
            IArrayElementInserter<string> elementInserter)
        {
            this.amountColumnIndex = amountColumnIndex;
            this.currencyColumnIndex = currencyColumnIndex;
            this.targetColumnIndex = targetColumnIndex;
            this.gbpValueSelector = gbpValueSelector;
            this.elementInserter = elementInserter;
        }

        public string[] Rewrite(string[] source)
        {
            if (targetColumnIndex > source.Length)
                throw new Exception($"The row is too short to add an item at a specified position ({targetColumnIndex}");

            var newValue = gbpValueSelector.GetValue(source[currencyColumnIndex], source[amountColumnIndex]);

            return elementInserter.Insert(source, targetColumnIndex, newValue);
        }
    }
}