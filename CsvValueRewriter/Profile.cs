using Andy.Csv.Transformation.Row.Filter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public static class Profile
    {
        private class Key
        {
            internal const string DateRewriter = "DateRewriter";
            internal const string TheCurrencyAmountThing = "TheCurrencyAmountThing";
            internal const string ColumnReducer = "ColumnReducer";
            internal const string ColumnInserter = "ColumnInserter";
            internal const string InvertedSingleValueFilter = "InvertedSingleValueFilter";
        }

        public static IDocumentTransformer[] GetTransformerChain(Settings settings, string profileName)
        {
            var rewriterChain = GetRewriterNames(settings, profileName);

            return rewriterChain
                .Select(name => GetRewriter(name, settings.Transformers))
                .ToArray();
        }

        private static IDocumentTransformer GetRewriter(
            string name,
            Settings.TransformationSettings transformationSettings)
        {
            switch (name)
            {
                case Key.DateRewriter:
                    return Build_DateRewriter(transformationSettings.DateRewriter);
                case Key.TheCurrencyAmountThing:
                    return Build_TheCurrencyAmountThing(transformationSettings.TheCurrencyAmountThing);
                case Key.ColumnReducer:
                    return Build_ColumnReducer(transformationSettings.ColumnReducer);
                case Key.ColumnInserter:
                    return Build_ColumnInserter(transformationSettings.ColumnInserter);
                case Key.InvertedSingleValueFilter:
                    return Build_InvertedSingleRowValueEvaluator(transformationSettings.InvertedSingleValueFilter);
                default:
                    throw new NotImplementedException($"Value: {name}");
            }
        }

        private static string[] GetRewriterNames(Settings settings, string profileName)
        {
            if (!settings.Profiles.Any())
                throw new Exception($"Transformation profile {profileName} does not exist");

            if (string.IsNullOrEmpty(profileName))
                return settings.Profiles.First().Value;

            string[] result;
            if (settings.Profiles.TryGetValue(profileName, out result))
                return result;

            throw new Exception("Now matching transformation profile has been found");
        }

        private static IDocumentTransformer Build_DateRewriter(Settings.TransformationSettings.DateRewriterSettings settings)
        {
            IRowTransformer rowRewriter = new SingleValueTransformer(
                    settings.TargetColumnIndex,
                    new DateTransformer(settings.SourceFormat, settings.TargetFormat));

            return new RowTransformer(rowRewriter);
        }

        private static IDocumentTransformer Build_TheCurrencyAmountThing(Settings.TransformationSettings.CurrencyAmountThingSettings settings)
        {
            IRowTransformer rowRewriter = new CurrencyAmount_CantThinkOfName(
                settings.AmountColumnIndex,
                settings.CurrencyColumnIndex,
                settings.ResultAmountColumnIndex,
                new TargetCurrencyValueSelector(settings.TargetCurrency));

            return new RowTransformer(rowRewriter);
        }

        private static IDocumentTransformer Build_ColumnReducer(Settings.TransformationSettings.ColumnReducerSettings settings)
        {
            var rowRewriter = new ColumnReducer(settings.TargetColumnIndexes);

            return new RowTransformer(rowRewriter);
        }

        private static IDocumentTransformer Build_ColumnInserter(Settings.TransformationSettings.ColumnInserterSettings settings)
        {
            var rowRewriter = new ColumnInserter(
                settings.TargetColumnIndex,
                new ArrayElementInserter<string>());

            return new RowTransformer(rowRewriter);
        }

        private static IDocumentTransformer Build_InvertedSingleRowValueEvaluator(Settings.TransformationSettings.InvertedSingleRowValueFilterSettings settings)
        {
            var matcher = new InvertedSingleRowValueEvaluator(
                settings.TargetColumnIndex,
                settings.TargetValue);

            return new RowFilter(matcher);
        }
    }
}