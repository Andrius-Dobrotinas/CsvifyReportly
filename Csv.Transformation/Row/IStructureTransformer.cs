using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Modifies the structure of a CSV document by adding/removing and/or moving columns around
    /// </summary>
    public interface IStructureTransformer : IRowTransformer
    {
        /// <summary>
        /// Adds/removes/moves header cells.
        /// In case of addition of new cells, this method makes sure the new cell's has a value (column name).
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        string[] TransformHeader(string[] cells);
    }
}