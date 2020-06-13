using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row
{
    /// <summary>
    /// Indicates any sort of problem with the structure of a CSV document
    /// </summary>
    public abstract class StructureException : Exception
    {
        public StructureException(string msg) : base(msg)
        {

        }
    }

    /// <summary>
    /// Means a column with a specified name does not exist in a document
    /// </summary>
    public class NoColumnException : StructureException
    {
        public NoColumnException(string columnName) : base(@$"Column ""{columnName}"" does not exist")
        {

        }
    }
}