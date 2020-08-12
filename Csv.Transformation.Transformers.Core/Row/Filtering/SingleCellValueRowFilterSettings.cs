using Andy.Csv.Transformation.Comparison.String;
using System;

namespace Andy.Csv.Transformation.Row.Filtering
{
    public class SingleCellValueRowFilterSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }
        public string TargetValue { get; set; }
        public bool IsCaseSensitive { get; set; }
        public ValueComparisonMethod Method { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return BuildSingleCellValueEvaluatorFactory();
        }

        protected SingleCellValueEvaluatorFactory BuildSingleCellValueEvaluatorFactory()
        {
            var name = this.GetDescription();

            var comparer = StraightforwardValueComparerFactory.Build(
                            TargetValue,
                            !IsCaseSensitive,
                            Method);

            return new SingleCellValueEvaluatorFactory(
                name,
                TargetColumnName,
                comparer);
        }
    }
}