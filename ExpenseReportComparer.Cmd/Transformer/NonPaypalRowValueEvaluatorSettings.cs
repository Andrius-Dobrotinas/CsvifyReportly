using Andy.Csv.Transformation.Row;
using Andy.Csv.Transformation.Row.Document.Cmd.Transformer;
using Andy.ExpenseReport.Comparison.Filtering.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparer.Cmd.Transformer
{
    public class NonPaypalRowValueEvaluatorSettings : TransformerSettings
    {
        public int TargetColumnIndex { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new NonPaypalRowValueEvaluatorFactory(
                            TargetColumnIndex,
                            new PaypalTransactionSpotter());
        }
    }
}