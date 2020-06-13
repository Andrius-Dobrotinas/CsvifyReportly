using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public class CurrencyAmount_CantThinkOfNameFactory
        : IRowTransformerFactory<CurrencyAmount_CantThinkOfName>
    {
        private readonly string amountColumnName;
        private readonly string currencyColumnName;
        private readonly string targetColumnName;

        private readonly ITargetCurrencyValueSelector valueSelector;

        public CurrencyAmount_CantThinkOfNameFactory(
            string amountColumnName,
            string currencyColumnName,
            string targetColumnName,
            ITargetCurrencyValueSelector valueSelector)
        {
            this.amountColumnName = amountColumnName ?? throw new ArgumentNullException(nameof(amountColumnName));
            this.currencyColumnName = currencyColumnName ?? throw new ArgumentNullException(nameof(currencyColumnName));
            this.targetColumnName = targetColumnName ?? throw new ArgumentNullException(nameof(targetColumnName));
            this.valueSelector = valueSelector ?? throw new ArgumentNullException(nameof(valueSelector));
        }

        public CurrencyAmount_CantThinkOfName Build(IDictionary<string, int> columnIndexes)
        {
            int amountColumnIndex = Column.GetOrThrow(columnIndexes, amountColumnName);
            int currencyColumnIndex = Column.GetOrThrow(columnIndexes, currencyColumnName);
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new CurrencyAmount_CantThinkOfName(
                amountColumnIndex,
                currencyColumnIndex,
                targetColumnIndex,
                valueSelector);
        }
    }
}