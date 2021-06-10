﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExpenseReportComparer.Win
{
    public static class JsonFileUtil
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