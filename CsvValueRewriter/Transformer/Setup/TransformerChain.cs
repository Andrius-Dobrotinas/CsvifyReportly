using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document.Cmd.Transformer.Setup
{
    public static class TransformerChain
    {
        //todo: move this to a new project because it's used by another Cmd project
        public static IEnumerable<TransformerSettings> GetTransformerSettingsChain(
            IDictionary<string, TransformerSettings[]> transformationProfiles,
            string profileName)
        {
            if (!transformationProfiles.Any())
                throw new Exception("There are no transformation profiles configured");

            if (string.IsNullOrEmpty(profileName))
                return transformationProfiles.First().Value;

            if (transformationProfiles.ContainsKey(profileName) == false)
                throw new Exception("No matching transformation profile has been found");

            return transformationProfiles[profileName];
        }

        public static IDocumentTransformer BuildTransformer(
            TransformerSettings settings,
            IDocumentTransformerFactoryBuilder transformerFactory)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (transformerFactory == null) throw new ArgumentNullException(nameof(transformerFactory));

            var factory = settings.BuildFactory();

            if (factory is IRowTransformerFactory<ICellContentTransformer> fact)
                return transformerFactory.BuildCellContentTransformerFactory(fact);
            if (factory is IRowTransformerFactory<IStructureTransformer> fact2)
                return transformerFactory.BuildStructureTransformerFactory(fact2);
            if (factory is IDocumentTransformerFactory<Filtering.IRowMatchEvaluator> fact3)
                return transformerFactory.BuildRowFiltererFactory(fact3);

            throw new NotSupportedException($"Type: {settings.GetType().FullName}");
        }

        public static IList<IDocumentTransformer> BuildTransformers(
            IEnumerable<TransformerSettings> settings,
            IDocumentTransformerFactoryBuilder documentTransformerFactory)
        {
            return settings
                .Select(config => BuildTransformer(config, documentTransformerFactory))
                .ToArray();
        }

        public static IList<IDocumentTransformer> GetTransformerChain(
            IDictionary<string, TransformerSettings[]> transformationProfiles,
            string profileName,
            IDocumentTransformerFactoryBuilder documentTransformerFactory)
        {
            IEnumerable<TransformerSettings> transformerSettings = GetTransformerSettingsChain(transformationProfiles, profileName);

            return BuildTransformers(transformerSettings, documentTransformerFactory);
        }
    }
}