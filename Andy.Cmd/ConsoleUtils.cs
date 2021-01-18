using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Cmd
{
    public static class ConsoleUtils
    {
        private static IEnumerable<string> UnpackMessages(Exception e)
        {
            var result = new string[] { e.Message };

            if (e.InnerException == null)
                return result;

            var msgs = UnpackMessages(e.InnerException);
            return result.Concat(msgs);
        }

        public static void PrintErrorDetails(Exception e)
        {
            var msgs = UnpackMessages(e);
            foreach (var msg in msgs)
                Console.Error.WriteLine(msg);
        }
    }
}
