using System;
using System.Collections.Generic;

namespace Andy.Cmd
{
    public class ArgumentSplitter
    {
        private readonly char separator;

        public ArgumentSplitter(char separator)
        {
            this.separator = separator;
        }

        public KeyValuePair<string, string> ParseArgument(string argument)
        {
            var separatorIndex = argument.IndexOf(separator);
            if (separatorIndex == -1)
            {
                return new KeyValuePair<string, string>(argument, null);
            }

            var name = argument.Substring(0, separatorIndex);
            var value = argument.Substring(separatorIndex + 1);

            return new KeyValuePair<string, string>(name, value);
        }
    }
}