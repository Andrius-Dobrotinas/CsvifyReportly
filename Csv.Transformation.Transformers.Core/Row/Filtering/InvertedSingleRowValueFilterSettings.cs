using System;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedSingleRowValueFilterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }
        public string TargetValue { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            var name = this.GetDescription();

            return new InvertedSingleCellValueEvaluatorFactory(
                name,
                new SingleCellValueEvaluatorFactory(
                    name,
                    TargetColumnName,
                        new StraightforwardValueComparer(TargetValue)));
        }
    }
}