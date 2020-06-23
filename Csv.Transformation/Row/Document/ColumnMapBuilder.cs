using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    /// <summary>
    /// Creates a map between between column names and their zero-based positions in the file
    /// </summary>
    public interface IColumnMapBuilder
    {
        /// <param name="columnNames">A sequence of column names, in the order that they appear in the source</param>
        IDictionary<string, int> GetColumnIndexMap(string[] columnNames);
    }

    public class ColumnMapBuilder : IColumnMapBuilder
    {
        public IDictionary<string, int> GetColumnIndexMap(string[] columnNames)
        {
            var result = new Dictionary<string, int>(columnNames.Length);

            for (int i = 0; i < columnNames.Length; i++)
            {
                result.Add(columnNames[i], i);
            }

            return result;
        }
    }
}