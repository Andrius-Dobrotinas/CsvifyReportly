using Andy.ExpenseReport.Comparison;
using Andy.ExpenseReport.Comparison.Csv.Statement;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public class MyCustomComparerSettings : ComparerSettings
    {
        public IDictionary<string, string[]> MerchantNameMap { get; set; }
        public int DateTolerance { get; set; }

        public override IItemComparer<
            StatementEntryWithSourceData,
            StatementEntryWithSourceData> BuildComparer()
        {
            return 
                new Comparison.Statement.ItemComparer(
                    new Comparison.Statement.MerchantNameComparer(
                        new Comparison.Statement.Bank.MerchanNameVariationComparer(this.MerchantNameMap),
                        new Comparison.Statement.StraighforwardDetailsComparer()),
                    new Comparison.Statement.Bank.InvertedAmountComparer(),
                    new Comparison.Statement.Bank.TolerantDateComparer(this.DateTolerance));
        }
    }
}