using System;
using System.Collections.Generic;

namespace Andy.Csv.Serialization
{
    public interface IRowParser
    {
        string[] Parse(string line);
    }
}