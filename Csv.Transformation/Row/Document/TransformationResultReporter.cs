using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row.Document
{
    public class TransformationResultReporter : ResultReporter
    {
        public TransformationResultReporter(IStringWriter stringWriter)
            : base(stringWriter)
        {

        }

        protected override void ReportDifferences(CsvDocument before, CsvDocument after)
        {            
            if (HaveHeaderCellsChanged(before.HeaderCells, after.HeaderCells))
            {
                stringWriter.WriteLine("Columns have been added/removed. The document is now made up of these columns:");
                stringWriter.WriteLine($"* {Stringicize(after.HeaderCells)}");
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
    }
}