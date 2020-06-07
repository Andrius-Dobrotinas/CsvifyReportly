using Andy.Csv.Rewrite.Rewriters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Rewrite
{
    public static class RewriterChain
    {
        private enum RewriterKey
        {
            DateRewriter,
            TheCurrencyAmountThing
        }

        public static IList<ICsvRewriter> GetRewriterChain(Settings settings, string chainName)
        {
            var rewriterChain = GetRewriterNames(settings, chainName);

            var csvRewriters = new List<ICsvRewriter>(rewriterChain.Length);

            if (rewriterChain.Contains(nameof(RewriterKey.DateRewriter)))
                csvRewriters.Add(GetDateRewriter(settings.Rewriters.DateRewriter));

            if (rewriterChain.Contains(nameof(RewriterKey.TheCurrencyAmountThing)))
                csvRewriters.Add(GetTheCurrencyAmountThing(settings.Rewriters.TheCurrencyAmountThing));

            return csvRewriters;
        }

        private static string[] GetRewriterNames(Settings settings, string targetChainName)
        {
            if (settings.RewriterChains.Count == 0) throw new Exception("There are no rewriter chains configured");

            if (string.IsNullOrEmpty(targetChainName))
                return settings.RewriterChains.FirstOrDefault().Value;

            string[] result;
            if (settings.RewriterChains.TryGetValue(targetChainName, out result))
                return result;

            throw new Exception($"Rewriter chain {targetChainName} does not exist");
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