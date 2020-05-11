using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Cmd
{
    public static class ArgumentParser
    {
        public static IDictionary<string, string> ParseArguments(string[] args)
        {
            var argParser = new ArgumentSplitter('=');

            return args.Select(argParser.ParseArgument)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value);
        }
    }
}