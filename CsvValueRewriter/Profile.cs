﻿using Andy.Csv.Transformation.Row.Filter;
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

            //todo: it's a good time to start using an IOC container
            var dataTransformerFactory = new CellContentTransformerFactory(
                new ColumnMapBuilder(),
                new CellContentTransformationRunner(
                    new Row.CellContentTransformationRunner()));

            return rewriterChain
                .Select(name => GetRewriter(name, settings.Transformers, dataTransformerFactory))
                .ToArray();
        }

        private static IDocumentTransformer GetRewriter(
            string name,
            Settings.TransformationSettings transformationSettings,
            CellContentTransformerFactory dataTransformerFactory)
        {
            switch (name)
            {
                case Key.DateRewriter:
                    return dataTransformerFactory.Build(
                        CellContentTransformer.Build_DateRewriter(transformationSettings.DateRewriter));
                case Key.TheCurrencyAmountThing:
                    return dataTransformerFactory.Build(
                        CellContentTransformer.Build_TheCurrencyAmountThing(transformationSettings.TheCurrencyAmountThing));
                case Key.ColumnReducer:
                    return Build_ColumnReducer(transformationSettings.ColumnReducer);
                case Key.ColumnInserter:
                    return Build_ColumnInserter(transformationSettings.ColumnInserter);
                case Key.InvertedSingleValueFilter:
                    return Build_InvertedSingleRowValueEvaluator(transformationSettings.InvertedSingleValueFilter);
                default:
                    throw new NotSupportedException($"Value: {name}");
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

        private static IDocumentTransformer Build_ColumnReducer(Settings.TransformationSettings.ColumnReducerSettings settings)
        {
            throw new NotImplementedException("In progress");
            var rowRewriter = new ColumnReducer(settings.TargetColumnIndexes);

            //return new RowTransformer(columnMapBuilder);
        }

        private static IDocumentTransformer Build_ColumnInserter(
            Settings.TransformationSettings.ColumnInserterSettings settings)
        {
            throw new NotImplementedException("In progress");
            IRowTransformerFactory<IStructureTransformer> transformerFactory = new ColumnInserterFactory(
                null, //settings.TargetColumnIndex, // todo
                new ArrayElementInserter<string>());
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