using Andy.Csv.Transformation.Row;
using Andy.Csv.Transformation.Row.Filtering;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Transformation.Csv.Filtering.Statement.Bank
{
    /// <summary>
    /// Selects Non-PayPal rows
    /// </summary>
    public class NonPaypalRowSelectorSettings : TransformerSettings
    {
        public string TargetColumnName { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            var name = this.GetDescription();
            return new InvertedRowMatchEvaluatorFactory(
                        name,
                        new SingleCellValueEvaluatorFactory(
                            name,
                            TargetColumnName,
                            new PaypalTransactionSpotter()));
        }
    }
}