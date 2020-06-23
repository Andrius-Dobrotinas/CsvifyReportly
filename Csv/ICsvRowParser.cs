using System;
using System.Collections.Generic;

namespace Andy.Csv
{
    public interface ICsvRowParser
    {
        string[] Parse(string line);
    }
}