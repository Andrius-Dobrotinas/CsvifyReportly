using Andy.Csv.Transformation.Row;
using Andy.Csv.Transformation.Row.Filtering;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Transformation.Csv.Filtering.Statement.Bank
{
    public class NonPaypalRowValueEvaluatorSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            var name = this.GetDescription();
            return new InvertedSingleCellValueEvaluatorFactory(
                        name,
                        new SingleCellValueEvaluatorFactory(
                            name,
                            TargetColumnName,
                            new PaypalTransactionSpotter()));
        }
    }
}