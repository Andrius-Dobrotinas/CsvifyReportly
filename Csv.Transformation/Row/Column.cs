using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    public static class Column
    {
        /// <summary>
        /// Gets an index for a given column, or throws an exception if not found
        /// </summary>
        public static int GetOrThrow(IDictionary<string, int> headerIndexes, string targetColumnName)
        {
            int index;
            if (headerIndexes.TryGetValue(targetColumnName, out index))
                return index;

            throw new NoColumnException(targetColumnName);
        }
    }
}