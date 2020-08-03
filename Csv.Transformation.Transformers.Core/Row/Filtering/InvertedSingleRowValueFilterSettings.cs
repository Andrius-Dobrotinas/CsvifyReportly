using System;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class InvertedSingleRowValueFilterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }
        public string TargetValue { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new InvertedSingleCellValueEvaluatorFactory(
                TargetColumnName,
                TargetValue);
        }
    }
}