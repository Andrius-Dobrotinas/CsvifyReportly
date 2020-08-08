using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document
{
    // this should be in a separate project, but there's no point in creating a separate project for just this one file
    public class ConsoleWriter : IStringWriter
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}