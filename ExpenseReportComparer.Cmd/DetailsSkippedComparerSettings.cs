using Andy.ExpenseReport.Comparison;
using Andy.ExpenseReport.Comparison.Csv.Statement;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class DetailsSkippedComparerSettings : ComparerSettings
    {
        public int DateTolerance { get; set; }
        public ExpenseReport.Comparison.Statement.Bank.DateComparisonDirection Direction { get; set; }

        public override IItemComparer<
            StatementEntryWithSourceData,
            StatementEntryWithSourceData> BuildComparer()
        {
            return 
                new Comparison.Statement.ItemComparer(
                    new Comparison.Statement.AlwaysEqualComparer(),
                    new Comparison.Statement.Bank.InvertedAmountComparer(),
                    new Comparison.Statement.Bank.TolerantDateComparer(
                        this.DateTolerance,
                        this.Direction));
        }
    }
}