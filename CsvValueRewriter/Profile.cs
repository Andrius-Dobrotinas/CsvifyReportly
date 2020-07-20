using Andy.Csv.Transformation.Row.Document.Cmd.Configuration;
using Andy.Csv.Transformation.Row.Document.Cmd.Configuration.Transformer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public static class Profile
    {
        public static IDocumentTransformer[] GetTransformerChain(
            Settings settings,
            string profileName)
        {
            IEnumerable<TransformerSettings> transformerSettings = GetTransformerSettingsChain(settings.Profiles, profileName);

            //todo: maybe it's time to start using an IOC container?
            var rowTransformer = new RowTransformationRunner();
            var dataTransformerFactory = new DocumentTransformerFactory(
                new ColumnMapBuilder(),
                new CellContentTransformationRunner(
                    rowTransformer),
                new StructureTransformationRunner(
                    rowTransformer),
                new RowFilterRunner(
                    new Filtering.RowFilter()));

            return transformerSettings
                .Select(x => GetTransformer(x, dataTransformerFactory))
                .ToArray();
        }

        //todo: move this to a new project because it's used by another Cmd project
        public static IEnumerable<TransformerSettings> GetTransformerSettingsChain(
            IDictionary<string, TransformerSettings[]> profiles,
            string profileName)
        {
            if (!profiles.Any())
                throw new Exception("There are no transformation profiles configured");

            if (string.IsNullOrEmpty(profileName))
                return profiles.First().Value;

            if (profiles.ContainsKey(profileName) == false)
                throw new Exception("No matching transformation profile has been found");

            return profiles[profileName];
        }

        private static IDocumentTransformer GetTransformer(
            TransformerSettings settings,
            DocumentTransformerFactory transformerFactory)
        {
            var type = settings.GetType();

            if (type == typeof(DateRewriterSettings))
                return transformerFactory.Build(
                    CellContentTransformer.Build_DateRewriter((DateRewriterSettings)settings));

            if (type == typeof(AmountInverterSettings))
                return transformerFactory.Build(
                    CellContentTransformer.Build_AmountInverter((AmountInverterSettings)settings));

            if (type == typeof(CurrencyAmountThingSettings))
                return transformerFactory.Build(
                    CellContentTransformer.Build_TheCurrencyAmountThing((CurrencyAmountThingSettings)settings));

            if (type == typeof(ColumnReducerSettings))
                return transformerFactory.Build(
                    StructureTransformer.Build_ColumnReducer((ColumnReducerSettings)settings));

            if (type == typeof(ColumnInserterSettings))
                return transformerFactory.Build(
                    StructureTransformer.Build_ColumnInserter((ColumnInserterSettings)settings));

            if (type == typeof(InvertedSingleRowValueFilterSettings))
                return transformerFactory.Build(
                    Build_InvertedSingleRowValueEvaluator((InvertedSingleRowValueFilterSettings)settings));

            throw new NotSupportedException($"Type: {type.FullName}");
        }

        private static IDocumentTransformerFactory<Filtering.IRowMatchEvaluator> Build_InvertedSingleRowValueEvaluator(
            InvertedSingleRowValueFilterSettings settings)
        {
            return new Filtering.InvertedSingleRowValueEvaluatorFactory(
                settings.TargetColumnName,
                settings.TargetValue);
        }
    }
}