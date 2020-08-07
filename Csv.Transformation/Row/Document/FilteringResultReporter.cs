using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class FilteringResultReporter : ResultReporter
    {
        public FilteringResultReporter(IStringWriter stringWriter)
            : base(stringWriter)
        {

        }

        protected override void ReportDifferences(CsvDocument before, CsvDocument after)
        {
            if (after.ContentRows.Length != before.ContentRows.Length)
            {
                stringWriter.WriteLine("The following rows have been filtered out:");

                /* since it's clear this was a filter operation, it means that
                 * rows or their content have not been altered in anyway, and that
                 * means that is safe to perform the referential equality */
                foreach (var row in before.ContentRows.Except(after.ContentRows))
                    stringWriter.WriteLine(Stringicize(row));
            }
            else
            {
                stringWriter.WriteLine("No rows have been filtered out");
            }
        }
    }
}