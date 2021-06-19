using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.ExpenseReport.Comparer.Win
{
    public static class StatePersistence
    {
        public static void SaveState(State state, FileInfo filePath)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            var jsonString = JsonConvert.SerializeObject(state, settings);

            File.WriteAllText(filePath.FullName, jsonString);
        }

        public static State ReadState(FileInfo filePath)
        {
            return Andy.Cmd.JasonFileParser.ParseContents<State>(filePath);
        }
    }
}