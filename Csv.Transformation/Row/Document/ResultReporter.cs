using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public interface IResultReporter
    {
        void ReportStart(string transformerName);
        void ReportFinish(CsvDocument before, CsvDocument after);
    }

    public class ResultReporter : IResultReporter
    {
        public void ReportStart(string transformerName)
        {
            Console.WriteLine(@$"Running transformer ""{transformerName}""");
        }

        public void ReportFinish(CsvDocument before, CsvDocument after)
        {
            Console.WriteLine("Transformer finished");

            // this means this was the filtering operation
            if (after.ContentRows.Length != before.ContentRows.Length)
            {
                Console.WriteLine("The following rows have been filtered out:");

                /* since it's clear this was a filter operation, it means that
                 * rows or their content have not been altered in anyway, and that
                 * means that is safe to perform the referential equality */
                foreach (var row in before.ContentRows.Except(after.ContentRows))
                    Console.WriteLine(Stringicize(row));
            }

            if (HaveHeaderCellsChanged(before.HeaderCells, after.HeaderCells))
            {
                Console.WriteLine("Columns have been added/removed. The document is now made up of these columns:");
                Console.WriteLine(Stringicize(after.HeaderCells));
            }
        }

        private bool HaveHeaderCellsChanged(string[] before, string[] after)
        {
            if (after.Length != before.Length)
                return true;

            for (int i = 0; i < before.Length; i++)
                if (before[i] != after[i])
                    return true;

            return false;
        }

        private string Stringicize(string[] row)
        {
            return string.Join('|', row);
        }
    }
}