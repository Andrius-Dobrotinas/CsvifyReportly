using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    public class CsvDocument
    {
        /// <summary>
        /// Column names in the order that the data appears in the source in
        /// </summary>
        public string[] HeaderCells { get; set; }

        /// <summary>
        /// Actual data rows
        /// </summary>
        public string[][] ContentRows { get; set; }
    }
}