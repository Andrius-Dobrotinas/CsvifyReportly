using Andy.Csv.Transformation.Row.Document.Cmd.Transformer;
using Andy.Csv.Transformation.Row.Document.Cmd.Transformer.Setup;
using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public static class Profile
    {
        public static IList<IDocumentTransformer> GetTransformerChain(
            IDictionary<string, TransformerSettings[]> transformationProfiles,
            string profileName)
        {
            var rowTransformer = new RowTransformationRunner();
            var documentTransformerFactory = new DocumentTransformerFactoryBuilder(
                new ColumnMapBuilder(),
                new CellContentTransformationRunner(
                    rowTransformer),
                new StructureTransformationRunner(
                    rowTransformer),
                new RowFilterRunner(
                    new Filtering.RowFilter()));

            return TransformerChain.GetTransformerChain(
                transformationProfiles,
                profileName,
                documentTransformerFactory);
        }
    }
}