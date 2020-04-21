﻿using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Cmd.CommandLine
{
    public class ArgumentParser
    {
        private readonly char separator;

        public ArgumentParser(char separator)
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