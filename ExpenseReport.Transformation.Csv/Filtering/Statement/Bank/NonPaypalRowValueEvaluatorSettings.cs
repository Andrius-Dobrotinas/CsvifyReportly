using Andy.Csv.Transformation.Row;
using Andy.ExpenseReport.Comparison.Filtering.Statement.Bank;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Transformation.Csv.Filtering.Statement.Bank
{
    public class NonPaypalRowValueEvaluatorSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new NonPaypalRowValueEvaluatorFactory(
                            this.GetDescription(),
                            TargetColumnName,
                            new PaypalTransactionSpotter());
        }
    }
}