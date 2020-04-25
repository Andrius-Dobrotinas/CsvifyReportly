using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd.Bank
{
    public class ExpenseReportParameters<TColumnIndexMap1, TColumnIndexMap2>
        : Comparison.Csv.CsvStream.Parameters<TColumnIndexMap1, TColumnIndexMap2>
    {
        public IDictionary<string, string[]> MerchantNameMap { get; set; }
    }
}