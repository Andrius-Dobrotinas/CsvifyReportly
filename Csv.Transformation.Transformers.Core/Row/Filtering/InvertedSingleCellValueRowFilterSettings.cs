using System;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedSingleCellValueRowFilterSettings : SingleCellValueRowFilterSettings
    {
        public override IDocumentTransformerFactory BuildFactory(ICultureSettings globalSettings)
        {
            var name = this.GetDescription();
            var factory = this.BuildSingleCellValueEvaluatorFactory();

            return new InvertedRowMatchEvaluatorFactory(
                name,
                factory);
        }
    }
}