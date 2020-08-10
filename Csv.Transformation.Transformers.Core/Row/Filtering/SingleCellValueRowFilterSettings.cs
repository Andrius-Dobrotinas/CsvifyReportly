using System;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class SingleCellValueRowFilterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }
        public string TargetValue { get; set; }
        public bool IsCaseInsensitive { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            var name = this.GetDescription();

            return new SingleCellValueEvaluatorFactory(
                name,
                TargetColumnName,
                    new StraightforwardValueComparer(TargetValue, IsCaseInsensitive));
        }
    }
}