﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public static class RewriterChain
    {
        private class Key
        {
            internal const string DateRewriter = "DateRewriter";
            internal const string TheCurrencyAmountThing = "TheCurrencyAmountThing";
            internal const string ColumnReducer = "ColumnReducer";
            internal const string ColumnInserter = "ColumnInserter";
        }

        public static IDocumentTransformer[] GetRewriterChain(Settings settings, string chainName)
        {
            var rewriterChain = GetRewriterNames(settings, chainName);

            return rewriterChain
                .Select(name => GetRewriter(name, settings.Rewriters))
                .ToArray();
        }

        private static IDocumentTransformer GetRewriter(string name, Settings.RewriterSettings rewriterSettings)
        {
            switch (name)
            {
                case Key.DateRewriter:
                    return Build_DateRewriter(rewriterSettings.DateRewriter);
                case Key.TheCurrencyAmountThing:
                    return Build_TheCurrencyAmountThing(rewriterSettings.TheCurrencyAmountThing);
                case Key.ColumnReducer:
                    return Build_ColumnReducer(rewriterSettings.ColumnReducer);
                case Key.ColumnInserter:
                    return Build_ColumnInserter(rewriterSettings.ColumnInserter);
                default:
                    throw new NotImplementedException($"Value: {name}");
            }
        }

        private static string[] GetRewriterNames(Settings settings, string targetChainName)
        {
            if (settings.RewriterChains.Count == 0)
            {
                if (string.IsNullOrEmpty(targetChainName))
                    return new string[0];
            }
            else
            {
                if (string.IsNullOrEmpty(targetChainName))
                    return settings.RewriterChains.FirstOrDefault().Value;

                string[] result;
                if (settings.RewriterChains.TryGetValue(targetChainName, out result))
                    return result;
            }

            throw new Exception($"Rewriter chain {targetChainName} does not exist");
        }

        private static IDocumentTransformer Build_DateRewriter(Settings.RewriterSettings.DateRewriterSettings settings)
        {
            IRowTransformer rowRewriter = new SingleValueTransformer(
                    settings.TargetColumnIndex,
                    new DateTransformer(settings.SourceFormat, settings.TargetFormat));

            return new DocumentRowTransformer(rowRewriter);
        }

        private static IDocumentTransformer Build_TheCurrencyAmountThing(Settings.RewriterSettings.CurrencyAmountThingSettings settings)
        {
            IRowTransformer rowRewriter = new CurrencyAmount_CantThinkOfName(
                settings.AmountColumnIndex,
                settings.CurrencyColumnIndex,
                settings.ResultAmountColumnIndex,
                new TargetCurrencyValueSelector(settings.TargetCurrency));

            return new DocumentRowTransformer(rowRewriter);
        }

        private static IDocumentTransformer Build_ColumnReducer(Settings.RewriterSettings.ColumnReducerSettings settings)
        {
            var rowRewriter = new ColumnReducer(settings.TargetColumnIndexes);

            return new DocumentRowTransformer(rowRewriter);
        }

        private static IDocumentTransformer Build_ColumnInserter(Settings.RewriterSettings.ColumnInserterSettings settings)
        {
            var rowRewriter = new ColumnInserter(
                settings.TargetColumnIndex,
                new ArrayElementInserter<string>());

            return new DocumentRowTransformer(rowRewriter);
        }
    }
}