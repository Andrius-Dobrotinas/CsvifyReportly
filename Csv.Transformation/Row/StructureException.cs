using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Means a column with a specified name does not exist in a document
    /// </summary>
    public class NoColumnException : StructureException
    {
        public NoColumnException(string columnName) : base(@$"Column ""{columnName}"" does not exist")
        {

        }
    }

    public class NonUniqueColumnException : StructureException
    {
        public NonUniqueColumnException(string columnName) : base(@$"Column ""{columnName}"" is already present in the collection")
        {
        }
    }

    /// <summary>
    /// Indicates that the number of cells was not what it was expected to be at a given point
    /// </summary>
    public class CellCountMismatchException : StructureException
    {
        public CellCountMismatchException(int expectedCellCount, int actualCellCount, int rowIndex) : base(@$"Expected {expectedCellCount} cells, got {actualCellCount}. Row index: {rowIndex}")
        {

        }
    }
}