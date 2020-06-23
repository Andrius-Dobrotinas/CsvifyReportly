using System;
using System.Collections.Generic;

namespace Andy.Csv
{
    /// <summary>
    /// Indicates any sort of problem with the structure of a CSV document
    /// </summary>
    public class StructureException : Exception
    {
        public StructureException(string msg) : base(msg)
        {

        }
    }
}