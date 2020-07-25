using Andy.Csv.Transformation.Row;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Filtering.Statement.Bank
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