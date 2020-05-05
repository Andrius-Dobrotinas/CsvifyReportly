using System;

namespace Andy.Csv
{
    public class MissingTokenException : Exception
    {
        public MissingTokenException(string msg) : base(msg)
        {

        }
    }
}