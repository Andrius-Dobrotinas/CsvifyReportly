using Andy.Csv.Transformation.Row.Document;
using Andy.ExpenseReport.Comparison.Filtering.Statement.Bank;
using Andy.ExpenseReport.Verifier.Cmd;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparer.Cmd
{
    public static class TransformationProfile
    {
        public static IDocumentTransformer Build(Csv.Transformation.Row.Document.Cmd.Configuration.Transformer.TransformerSettings settings)
        {
            var type = settings.GetType();

            if (type == typeof(NonPaypalRowValueEvaluatorSettings))
                return Build_NonPaypalRowValueEvaluator((NonPaypalRowValueEvaluatorSettings)settings);

            throw new NotSupportedException($"Type: {type.FullName}");
        }

        internal static RowFilterer Build_NonPaypalRowValueEvaluator(
            NonPaypalRowValueEvaluatorSettings settings)
        {
            return new RowFilterer(
                        new ColumnMapBuilder(),
                        new NonPaypalRowValueEvaluatorFactory(
                            settings.TargetColumnIndex,
                            new PaypalTransactionSpotter()),
                        new RowFilterRunner(
                            new Csv.Transformation.Row.Filtering.RowFilter()));
        }
    }
}