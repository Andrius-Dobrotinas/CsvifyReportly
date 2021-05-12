using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public static class ResultStringification
    {
        const int separatorColumnCount = 1;

        public static string[] StringyfyyResults(
            ComparisonResult result,
            string[] source1HeaderCells,
            string[] source2HeaderCells,
            char csvDelimiter,
            Andy.Csv.Serialization.IRowStringifier stringyfier)
        {
            var blankSource1Row = new string[source1HeaderCells.Length];
            var blankSource2Row = new string[source2HeaderCells.Length];

            var allRows = ResultAggregation.GetDataRows(
                result.Matches,
                result.MatchesSecondary,
                result.UnmatchedTransactions1,
                result.UnmatchedTransactions2,
                separatorColumnCount,
                blankSource1Row,
                blankSource2Row,
                source1HeaderCells,
                source2HeaderCells);

            return allRows
                .Select(row => stringyfier.Stringifififiify(row, csvDelimiter))
                .ToArray();
        }
    }
}