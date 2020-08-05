using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public class AmountInLocalCurrencyProducerFactory
        : IRowTransformerFactory<AmountInLocalCurrencyProducer>
    {
        private readonly string amountColumnName;
        private readonly string currencyColumnName;
        private readonly string targetColumnName;

        private readonly ITargetCurrencyValueSelector valueSelector;

        public AmountInLocalCurrencyProducerFactory(
            string name,
            string amountColumnName,
            string currencyColumnName,
            string targetColumnName,
            ITargetCurrencyValueSelector valueSelector)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.amountColumnName = amountColumnName ?? throw new ArgumentNullException(nameof(amountColumnName));
            this.currencyColumnName = currencyColumnName ?? throw new ArgumentNullException(nameof(currencyColumnName));
            this.targetColumnName = targetColumnName ?? throw new ArgumentNullException(nameof(targetColumnName));
            this.valueSelector = valueSelector ?? throw new ArgumentNullException(nameof(valueSelector));
        }

        public string Name { get; }

        public AmountInLocalCurrencyProducer Build(IDictionary<string, int> columnIndexes)
        {
            int amountColumnIndex = Column.GetOrThrow(columnIndexes, amountColumnName);
            int currencyColumnIndex = Column.GetOrThrow(columnIndexes, currencyColumnName);
            int targetColumnIndex = Column.GetOrThrow(columnIndexes, targetColumnName);

            return new AmountInLocalCurrencyProducer(
                amountColumnIndex,
                currencyColumnIndex,
                targetColumnIndex,
                valueSelector);
        }
    }
}