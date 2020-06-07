using Andy.Csv.Rewrite.Rewriters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Rewrite
{
    public static class RewriterChain
    {
        private enum RewriterKeys
        {
            DateRewriter,
            TheCurrencyAmountThing
        }

        public static IList<ICsvRewriter> GetRewriterChain(Settings settings)
        {
            var csvRewriters = new List<ICsvRewriter>(settings.RewriterChain.Length);

            if (settings.RewriterChain.Contains(nameof(RewriterKeys.DateRewriter)))
                csvRewriters.Add(GetDateRewriter(settings.Rewriters.DateRewriter));

            if (settings.RewriterChain.Contains(nameof(RewriterKeys.TheCurrencyAmountThing)))
                csvRewriters.Add(GetTheCurrencyAmountThing(settings.Rewriters.TheCurrencyAmountThing));

            return csvRewriters;
        }

        private static ICsvRewriter GetDateRewriter(Settings.RewriterSettings.DateRewriterSettings settings)
        {
            IRowRewriter rowRewriter = new RowSingleValueRewriter(
                    settings.TargetColumnIndex,
                    new Rewriters.DateRewriter(settings.SourceFormat, settings.TargetFormat));

            return new CsvRewriter(rowRewriter);
        }

        private static ICsvRewriter GetTheCurrencyAmountThing(Settings.RewriterSettings.CurrencyAmountThingSettings settings)
        {
            IRowRewriter rowRewriter = new CurrencyAmount_CantThinkOfName(
                settings.AmountColumnIndex,
                settings.CurrencyColumnIndex,
                settings.ResultAmountColumnIndex,
                new TargetCurrencyValueSelector(settings.TargetCurrency),
                new ArrayElementInserter<string>());

            return new CsvRewriter(rowRewriter);
        }
    }
}