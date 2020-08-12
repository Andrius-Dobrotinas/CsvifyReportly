using Andy.Csv.Transformation.Comparison.String;
using System;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class SingleCellValueRowFilterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }
        public string TargetValue { get; set; }
        public bool IsCaseInsensitive { get; set; }
        public ValueComparisonMethod Method { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            var name = this.GetDescription();

            var comparer = StraightforwardValueComparerFactory.Build(
                            TargetValue,
                            IsCaseInsensitive,
                            Method);

            return new SingleCellValueEvaluatorFactory(
                name,
                TargetColumnName,
                comparer);
        }
    }
}