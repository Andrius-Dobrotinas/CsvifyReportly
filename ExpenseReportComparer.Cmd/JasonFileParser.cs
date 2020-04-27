using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class JasonFileParser
    {
        public static T ParseContents<T>(FileInfo file)
        {
            using (var fs = file.OpenRead())
            {
                using (var reader = new StreamReader(fs))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                }
            }
        }
    }
}