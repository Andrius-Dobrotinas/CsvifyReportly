using System;
using System.Collections.Generic;

namespace Andy.Csv
{
    public interface IRowParser
    {
        string[] Parse(string line);
    }
}