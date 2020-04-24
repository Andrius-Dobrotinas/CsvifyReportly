using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class SettingsReader
    {
        public static T ReadSettings<T>(FileInfo settingsFile)
        {
            using (var fs = settingsFile.OpenRead())
            {
                using (var reader = new StreamReader(fs))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                }
            }
        }
    }
}