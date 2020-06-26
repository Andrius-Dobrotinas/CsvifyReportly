using System;

namespace Andy.Csv.Serialization
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException(string msg) : base(msg)
        {

        }
    }
}