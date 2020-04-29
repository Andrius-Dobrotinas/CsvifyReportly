using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public interface IReportingComparer
    {
        Stream Compare(
            Stream source1,
            Stream source2,
            char stream1CsvDelimiter,
            char stream2CsvDelimiter,
            char reportValueDelimiter);
    }
}