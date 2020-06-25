using System;

namespace Andy.Csv
{
    /// <summary>
    /// Means that a certain token was expected at a certain point but was not found
    /// </summary>
    public class MissingTokenException : Exception
    {
        public MissingTokenException(string msg) : base(msg)
        {

        }
    }
}