using Andy.ExpenseReport.Comparison;
using Andy.ExpenseReport.Comparison.Csv.Statement;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class MyCustomComparer2Settings : ComparerSettings
    {
        public int DateTolerance { get; set; }
        public ExpenseReport.Comparison.Statement.Bank.DateComparisonDirection Direction { get; set; }

        public override IItemComparer<
            StatementEntryWithSourceData,
            StatementEntryWithSourceData> BuildComparer()
        {
            return 
                new Comparison.Statement.ItemComparer(
                    new Comparison.Statement.MerchantNameStartsWithComparer(),
                    new Comparison.Statement.Bank.InvertedAmountComparer(),
                    new Comparison.Statement.Bank.TolerantDateComparer(
                        this.DateTolerance,
                        this.Direction));
        }
    }
}