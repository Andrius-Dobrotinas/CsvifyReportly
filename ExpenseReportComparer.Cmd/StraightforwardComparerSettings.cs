using Andy.ExpenseReport.Comparison;
using Andy.ExpenseReport.Comparison.Csv.Statement;
using Andy.ExpenseReport.Comparison.Statement;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class StraightforwardComparerSettings : ComparerSettings
    {
        public override IItemComparer<
            StatementEntryWithSourceData,
            StatementEntryWithSourceData> BuildComparer()
        {
            return 
                new ItemComparer(
                    new StraighforwardDetailsComparer(),
                    new AmountComparer(),
                    new DateComparer());
        }
    }
}