using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Setup
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
                    new Filtering.RowFilter()),
                new ResultReporter());

            return TransformerChain.GetTransformerChain(
                transformationProfiles,
                profileName,
                documentTransformerFactory);
        }
    }
}