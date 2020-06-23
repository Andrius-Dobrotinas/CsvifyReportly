using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Andy.Cmd
{
    public static class JasonFileParser
    {
        public static T ParseContents<T>(FileInfo file)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            using (var fs = file.OpenRead())
            {
                using (var reader = new StreamReader(fs))
                {
                    return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), settings);
                }
            }
        }
    }
}