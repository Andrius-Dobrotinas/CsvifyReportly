using System;

namespace Andy.Csv
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException(string msg) : base(msg)
        {

        }
    }
}