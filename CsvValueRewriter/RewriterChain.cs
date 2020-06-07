using Andy.Csv.Rewrite.Rewriters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Rewrite
{
    public static class RewriterChain
    {
        private class RewriterKey
        {
            internal const string DateRewriter = "DateRewriter";
            internal const string TheCurrencyAmountThing = "TheCurrencyAmountThing";
            internal const string ColumnReducer = "ColumnReducer";
        }

        public static IList<ICsvRewriter> GetRewriterChain(Settings settings, string chainName)
        {
            var rewriterChain = GetRewriterNames(settings, chainName);

            return rewriterChain
                .Select(name => GetRewriter(name, settings.Rewriters))
                .ToArray();
        }

        private static ICsvRewriter GetRewriter(string name, Settings.RewriterSettings rewriterSettings)
        {
            switch (name)
            {
                case RewriterKey.DateRewriter:
                    return BuildDateRewriter(rewriterSettings.DateRewriter);
                case RewriterKey.TheCurrencyAmountThing:
                    return BuildTheCurrencyAmountThing(rewriterSettings.TheCurrencyAmountThing);
                case RewriterKey.ColumnReducer:
                    return BuildColumnReducer(rewriterSettings.ColumnReducer);
                default:
                    throw new NotImplementedException($"Value: {name}");
            }
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

        private static ICsvRewriter BuildDateRewriter(Settings.RewriterSettings.DateRewriterSettings settings)
        {
            IRowRewriter rowRewriter = new RowSingleValueRewriter(
                    settings.TargetColumnIndex,
                    new DateRewriter(settings.SourceFormat, settings.TargetFormat));

            return new CsvRewriter(rowRewriter);
        }

        private static ICsvRewriter BuildTheCurrencyAmountThing(Settings.RewriterSettings.CurrencyAmountThingSettings settings)
        {
            IRowRewriter rowRewriter = new CurrencyAmount_CantThinkOfName(
                settings.AmountColumnIndex,
                settings.CurrencyColumnIndex,
                settings.ResultAmountColumnIndex,
                new TargetCurrencyValueSelector(settings.TargetCurrency),
                new ArrayElementInserter<string>());

            return new CsvRewriter(rowRewriter);
        }

        private static ICsvRewriter BuildColumnReducer(Settings.RewriterSettings.ColumnReducerSettings settings)
        {
            var rowRewriter = new ColumnReducer(settings.TargetColumnIndexes);

            return new CsvRewriter(rowRewriter);
        }
    }
}