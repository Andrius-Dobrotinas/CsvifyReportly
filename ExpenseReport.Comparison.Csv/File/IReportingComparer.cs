using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Comparison.Csv.File
{
    public interface IReportingComparer<TItem1, TItem2>
    {
        Stream Compare<TColumnIndexMap1, TColumnIndexMap2>(
            Stream source1,
            Stream source2,
            Parameters<TColumnIndexMap1, TColumnIndexMap2> settings);
    }
}